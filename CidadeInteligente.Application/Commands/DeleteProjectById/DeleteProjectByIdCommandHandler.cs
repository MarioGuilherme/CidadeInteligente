using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Projects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommandHandler(INotificationContext notification,
    IUnitOfWork unitOfWork,
    IFileStorage fileStorage,
    ILogger<DeleteProjectByIdCommandHandler> logger) : IRequestHandler<DeleteProjectByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(DeleteProjectByIdCommand request, CancellationToken cancellationToken)
    {
        Specification<Project> getProjectByIdSpec = ProjectSpecifications.GetById(request.ProjectId).Build();
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

        await _unitOfWork.Projects.DeleteByPredicateAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        await Task.WhenAll(project.Medias.Select(async m =>
        {
            try
            {
                await _fileStorage.DeleteFileAsync(m.FileName, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao remover arquivo {FileName} do projeto {ProjectId} já excluído", m.FileName, project.ProjectId);
            }
        }));

        return Unit.Value;
    }
}
