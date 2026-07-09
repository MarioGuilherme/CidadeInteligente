using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Projects;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public class UpdateProjectCommandHandler(INotificationContext notification,
    IUnitOfWork unitOfWork,
    IFileStorage fileStorage,
    ILogger<UpdateProjectCommandHandler> logger) : IRequestHandler<UpdateProjectCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Specification<Project> getProjectByIdSpec = ProjectSpecifications.GetById(request.ProjectId)
             .Include(p => p.Medias)
             .Include(p => p.InvolvedUsers)
             .Build();

        Project? project = await _unitOfWork.Projects.GetBySpecAsync(getProjectByIdSpec, cancellationToken);
        if (project is null)
        {
            _notification.AddNotification(NotificationType.ProjectNotFound, [request.ProjectId]);
            return null;
        }

        if (request.CurrentUserId != project.CreatedByUserId && !project.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId))
        {
            _notification.AddNotification(NotificationType.UserNotAuthorizedToModifyProject, [request.CurrentUserId, request.ProjectId]);
            return null;
        }

        int[] mediaIdsToKeep = [.. request.Medias.Where(m => m.MediaId is not null).Select(m => m.MediaId!.Value)];
        List<Media> mediasToRemove = [.. project.Medias.Where(m => !mediaIdsToKeep.Contains(m.MediaId))];
        ConcurrentBag<Media> uploadedMedias = [];
        List<(Media Entity, string Title, string? Description, long FileSize, Func<Stream>? StreamFactory)> mediaUpdates = [];

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            project.Update(request.AreaId,
                request.CourseId,
                request.Title,
                request.Description,
                request.StartedAt,
                request.FinishedAt);

            int[] currentUserIds = [.. project.InvolvedUsers.Select(u => u.UserId)];

            foreach (User toRemove in project.InvolvedUsers.Where(u => !request.InvolvedUsers.Contains(u.UserId)).ToList())
                project.InvolvedUsers.Remove(toRemove);

            foreach (int involvedUserId in request.InvolvedUsers.Where(iuId => !currentUserIds.Contains(iuId)))
            {
                Specification<User> getUserByIdSpec = UserSpecifications.GetById(involvedUserId).Build();
                User? newUserInvolved = await _unitOfWork.Users.GetBySpecAsync(getUserByIdSpec, cancellationToken);
                if (newUserInvolved is null)
                {
                    _notification.AddNotification(NotificationType.UserNotFound, [involvedUserId]);
                    continue;
                }

                project.InvolvedUsers.Add(newUserInvolved);
            }

            foreach (Media m in mediasToRemove)
                project.Medias.Remove(m);

            List<UpdateProjectCommand.UpdateMediaCommand> newMediaCommands = [.. request.Medias.Where(m => m.MediaId is null)];
            await Task.WhenAll(newMediaCommands.Select(async cmd =>
            {
                await using Stream stream = cmd.OpenStream.Invoke();
                string fileName = Guid.NewGuid().ToString("N");
                Media media = new(cmd.Title, cmd.Description, fileName, cmd.MimeType);
                await _fileStorage.UploadOrUpdateFileAsync(fileName, stream, cancellationToken);
                uploadedMedias.Add(media);
            }));

            foreach (Media media in uploadedMedias)
                project.Medias.Add(media);

            foreach (UpdateProjectCommand.UpdateMediaCommand? cmd in request.Medias.Where(m => m.MediaId is not null))
            {
                Media mediaDb = project.Medias.First(m => m.MediaId == cmd.MediaId!.Value);
                if (cmd.FileSize > 0)
                    mediaUpdates.Add((mediaDb, cmd.Title, cmd.Description, cmd.FileSize, cmd.OpenStream));
                mediaDb.Update(cmd.Title, cmd.Description, cmd.FileSize > 0 ? cmd.MimeType : null);
            }
        },
        onRollback: async ct =>
        {
            await Task.WhenAll(uploadedMedias.Select(async m =>
            {
                try
                {
                    await _fileStorage.DeleteFileAsync(m.FileName, ct);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao limpar upload {FileName}", m.FileName);
                }
            }));
        },
        cancellationToken);

        await Task.WhenAll(mediasToRemove.Select(async m =>
        {
            try
            {
                await _fileStorage.DeleteFileAsync(m.FileName, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao remover arquivo órfão {FileName}", m.FileName);
            }
        }));

        await Task.WhenAll(mediaUpdates
            .Where(u => u.StreamFactory is not null)
            .Select(async u =>
            {
                if (u.FileSize == 0) return;
                await using Stream stream = u.StreamFactory!();

                try
                {
                    await _fileStorage.UploadOrUpdateFileAsync(u.Entity.FileName, stream, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao atualizar conteúdo do arquivo {FileName}", u.Entity.FileName);
                }
            }));

        return Unit.Value;
    }
}
