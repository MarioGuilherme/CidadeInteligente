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

        if (!(request.CurrentUserId == projectDb.CreatorUserId || projectDb.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId)))
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
            if (newUserInvolved is null) continue;
            projectDb.InvolvedUsers.Add(newUserInvolved);
        }
        foreach (Media mediaDb in projectDb.Medias)
        {
            if (request.Medias.Any(mediaForm => mediaDb.MediaId == mediaForm.MediaId)) continue;
            await _fileStorage.DeleteFileAsync(mediaDb.FileName);
            _unitOfWork.Projects.DeleteMedia(mediaDb);
        }

        projectDb.Medias.Clear();
        foreach (UpdateProjectCommand.UpdateMediaCommand media in request.Medias)
        {
            if (media.MediaId is null)
            {
                projectDb.Medias.Add(new(media.Title,
                    media.Description,
                    _fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}.{media.Extension}", media.Base64!).Result,
                    (long)media.Size!));
                continue;
            }

            Media? mediaDb = await _unitOfWork.Projects.GetMediaById((long)media.MediaId);
            if (mediaDb is null) continue;

            if (media.Base64 is null)
                mediaDb.Update(media.Title, media.Description);
            else
            {
                await _fileStorage.UploadOrUpdateFileAsync(mediaDb.FileName, media.Base64);
                mediaDb.Update(media.Title, media.Description, (long)media.Size!);
            }
            projectDb.Medias.Add(mediaDb);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}
