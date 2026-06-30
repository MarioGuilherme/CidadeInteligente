using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<DeleteProjectByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(DeleteProjectByIdCommand request, CancellationToken cancellationToken)
    {
        Project? project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            Log.Warning("Project with ID {ProjectId} ​​not found.", request.ProjectId);
            _notification.AddNotification(NotificationType.ProjectNotFound, [request.ProjectId]);
            return null;
        }

        if (request.CurrentUserId != project.CreatedByUserId && !project.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId))
        {
            Log.Warning("User with ID {CurrentUserId} is not authorized to modify project with ID {ProjectId}.", request.CurrentUserId, request.ProjectId);
            _notification.AddNotification(NotificationType.UserNotAuthorizedToModifyProject, [request.ProjectId]);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await Task.WhenAll(project.Medias.Select(m => _fileStorage.DeleteFileAsync(m.FileName)));
        _unitOfWork.Projects.DeleteProject(project);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
