using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
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
        Project? projectDb = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, true);
        if (projectDb is null)
        {
            Log.Warning("Project with ID {ProjectId} ​​not found.", request.ProjectId);
            _notification.AddNotification(NotificationType.ProjectNotFound);
            return null;
        }

        if (request.CurrentUserId != projectDb.CreatedByUserId && !projectDb.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId))
        {
            Log.Warning("User with ID {CurrentUserId} is not authorized to modify project with ID {ProjectId}.", request.CurrentUserId, request.ProjectId);
            _notification.AddNotification(NotificationType.UserNotAuthorizedToModifyProject);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        projectDb.Update(request.AreaId,
            request.CourseId,
            request.Title,
            request.Description,
            request.StartedAt,
            request.FinishedAt);
        projectDb.InvolvedUsers.Clear();

        foreach (long userId in request.InvolvedUsers)
        {
            User? newUserInvolved = await _unitOfWork.Users.GetByIdAsync(userId, true);
            if (newUserInvolved is null)
            {
                Log.Warning("User with ID {UserId} was not found.", userId);
                _notification.AddNotification(NotificationType.UserNotFound, [userId]);
                continue;
            }
            projectDb.InvolvedUsers.Add(newUserInvolved);
        }

        foreach (Media mediaDb in projectDb.Medias)
        {
            if (request.Medias.Any(mediaForm => mediaForm.MediaId == mediaDb.MediaId)) continue;
            await _fileStorage.DeleteFileAsync(mediaDb.FileName);
            _unitOfWork.Projects.DeleteMedia(mediaDb);
        }

        projectDb.Medias.Clear();
        foreach (UpdateProjectCommand.UpdateMediaCommand mediaCommand in request.Medias)
        {
            await using Stream mediaStream = mediaCommand.OpenStream.Invoke();
            if (mediaCommand.MediaId is null)
            {
                Media media = new(mediaCommand.Title,
                    mediaCommand.Description,
                    await _fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}{mediaCommand.Extension}", mediaStream!));
                projectDb.Medias.Add(media);
                continue;
            }

            Media? mediaDb = await _unitOfWork.Projects.GetMediaById(mediaCommand.MediaId.Value);
            if (mediaDb is null)
            {
                Log.Warning("The media with ID {MediaId} was not found.", mediaCommand.MediaId);
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

            projectDb.Medias.Add(mediaDb);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}
