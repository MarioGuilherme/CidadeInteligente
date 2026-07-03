using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Core.Specifications;
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
        Specification<Project> specProject = SpecificationBuilder<Project>.Create()
            .Where(p => p.ProjectId == request.ProjectId)
            .AsEditable()
            .Build();

        Project? project = await _unitOfWork.Projects.GetBySpecAsync(specProject);
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

        await _unitOfWork.ExecuteInTransactionAsync(async _ =>
        {
            await _unitOfWork.Projects.DeleteAsync(project);
        }, cancellationToken: cancellationToken);

        await Task.WhenAll(project.Medias.Select(async m =>
        {
            try
            {
                await _fileStorage.DeleteFileAsync(m.FileName, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Falha ao remover arquivo {FileName} do projeto {ProjectId} já excluído", m.FileName, project.ProjectId);
            }
        }));

        return Unit.Value;
    }
}
