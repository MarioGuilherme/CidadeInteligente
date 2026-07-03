using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetProjectByIdQueryResult?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Project, GetProjectByIdQueryResult?> spec = SpecificationBuilder<Project>.Create()
            .Where(p => p.ProjectId == request.ProjectId)
            .WithProjection(p => new GetProjectByIdQueryResult(p.ProjectId,
                p.Title,
                p.Area.Description,
                p.AreaId,
                p.Course.Description,
                p.CourseId,
                p.Description,
                p.StartedAt,
                p.FinishedAt,
                p.InvolvedUsers.Select(iu => new GetProjectByIdQueryResult.ProjectUserViewModel(iu.UserId, iu.Name)),
                p.Medias.Select(m => new GetProjectByIdQueryResult.MediaDetailsViewModel(m.MediaId, m.Title, m.Description, m.FileName))));

        GetProjectByIdQueryResult? project = await _unitOfWork.Projects.GetBySpecAsync(spec);
        if (project is null)
        {
            _notification.AddNotification(NotificationType.ProjectNotFound, [request.ProjectId]);
            return null;
        }

        return project;
    }
}
