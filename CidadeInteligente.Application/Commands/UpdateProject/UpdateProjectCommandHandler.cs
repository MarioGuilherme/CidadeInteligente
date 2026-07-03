using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public class UpdateProjectCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<UpdateProjectCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        //Project? project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, true);
        Specification<Project> specProject = SpecificationBuilder<Project>.Create()
            .Include(p => p.Medias)
            .Where(p => p.ProjectId == request.ProjectId)
            .AsEditable()
            .Build();

        Project? project = await _unitOfWork.Projects.GetBySpecAsync(specProject);
        if (project is null)
        {
            _notification.AddNotification(NotificationType.ProjectNotFound);
            return null;
        }

        if (request.CurrentUserId != project.CreatedByUserId && !project.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId))
        {
            _notification.AddNotification(NotificationType.UserNotAuthorizedToModifyProject);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        project.Update(request.AreaId,
            request.CourseId,
            request.Title,
            request.Description,
            request.StartedAt,
            request.FinishedAt);
        project.InvolvedUsers.Clear();

        foreach (int userId in request.InvolvedUsers)
        {
            ////User? newUserInvolved = await _unitOfWork.Users.GetByIdAsync(userId, true);
            //var spec = SpecificationBuilder<User>.Create()
            //    .Where(u => u.UserId == userId)
            //    //.Include(u => u.Course)
            //    .WithProjection(u => new User(u.UserId,
            //        u.CourseId,
            //        u.Name,
            //        u.Email,
            //        u.Password,
            //        u.Role))
            //    .NoTracking()
            //    .Build();

            Specification<User> specNewInvolvedUser = SpecificationBuilder<User>.Create()
                .Where(u => u.UserId == userId)
                .AsEditable()
                .Build();

            User? newUserInvolved = await _unitOfWork.Users.GetBySpecAsync(specNewInvolvedUser);
            //var newUserInvolved = await _unitOfWork.Users.GetProjectionBySpecAsync(spec);
            if (newUserInvolved is null)
            {
                _notification.AddNotification(NotificationType.UserNotFound, [userId]);
                continue;
            }
            project.InvolvedUsers.Add(newUserInvolved);
        }

        foreach (Media mediaDb in project.Medias)
        {
            if (request.Medias.Any(mediaForm => mediaForm.MediaId == mediaDb.MediaId)) continue;
            await _fileStorage.DeleteFileAsync(mediaDb.FileName);
            _unitOfWork.Projects.DeleteMedia(mediaDb);
        }

        project.Medias.Clear();
        foreach (UpdateProjectCommand.UpdateMediaCommand mediaCommand in request.Medias)
        {
            await using Stream mediaStream = mediaCommand.OpenStream.Invoke();
            if (mediaCommand.MediaId is null)
            {
                string fileName = await _fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}{mediaCommand.Extension}", mediaStream!);
                Media media = new(mediaCommand.Title, mediaCommand.Description, fileName);
                project.Medias.Add(media);
                continue;
            }

            //Specification<Media> specMedia = SpecificationBuilder<Media>.Create()
            //    .Where(m => m.MediaId == mediaCommand.MediaId.Value)
            //    .AsEditable()
            //    .Build();

            //Media? mediaDb = await _unitOfWork.Projects.GetBySpecAsync(specMedia);
            //Media? mediaDb = await _unitOfWork.Projects.GetMediaById(mediaCommand.MediaId.Value);
            Media? mediaDb = project.Medias.FirstOrDefault(m => m.MediaId == mediaCommand.MediaId.Value);
            if (mediaDb is null)
            {
                _notification.AddNotification(NotificationType.MediaNotFound, [mediaCommand.MediaId]);
                continue;
            }

            if (mediaStream.Length == 0)
                mediaDb.Update(mediaCommand.Title, mediaCommand.Description);
            else
            {
                await _fileStorage.UploadOrUpdateFileAsync(mediaDb.FileName, mediaStream);
                mediaDb.Update(mediaCommand.Title, mediaCommand.Description);
            }

            project.Medias.Add(mediaDb);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}
