using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetProjectByIdQueryResult?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            Log.Warning("Project with ID {ProjectId} ​​not found.", request.ProjectId);
            _notification.AddNotification(NotificationType.ProjectNotFound);
            return null;
        }

        if (request.CurrentUserId != project.CreatedByUserId && !project.InvolvedUsers.Any(iu => iu.UserId == request.CurrentUserId))
        {
            Log.Warning("User with ID {CurrentUserId} is not authorized to modify project with ID {ProjectId}.", request.CurrentUserId, request.ProjectId);
            _notification.AddNotification(NotificationType.UserNotAuthorizedToModifyProject);
            return null;
        }

        return new(project.ProjectId,
            project.Title,
            project.Area.Description,
            project.AreaId,
            project.Course.Description,
            project.CourseId,
            project.Description!,
            project.StartedAt,
            project.FinishedAt,
            project.InvolvedUsers.Select(iu => new GetProjectByIdQueryResult.ProjectUserViewModel(iu.UserId, iu.Name)),
            project.Medias.Select(m => new GetProjectByIdQueryResult.MediaDetailsViewModel(m.MediaId, m.Title, m.Description, m.FileName)));
    }
}
