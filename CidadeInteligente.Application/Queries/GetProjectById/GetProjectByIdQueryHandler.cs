using CidadeInteligente.Application.Options;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Projects;
using MediatR;
using Microsoft.Extensions.Options;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork, IOptions<AzureStorageOptions> options) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _blobUrl = options.Value.BlobUrl!.TrimEnd('/');

    public async Task<GetProjectByIdQueryResult?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Project, GetProjectByIdQueryResult?> getProjectByIdSpec = ProjectSpecifications.GetById(request.ProjectId)
            .WithProjection<GetProjectByIdQueryResult>(p => new(p.ProjectId,
                p.Title,
                p.Area.Description,
                p.AreaId,
                p.Course.Description,
                p.CourseId,
                p.Description,
                p.StartedAt,
                p.FinishedAt,
                p.InvolvedUsers.Select(iu => new GetProjectByIdQueryResult.ProjectUserViewModel(iu.UserId, iu.Name)),
                p.Medias.Select(m => new GetProjectByIdQueryResult.MediaDetailsViewModel(m.MediaId,
                    m.Title,
                    m.Description,
                    $"{_blobUrl}/{m.FileName}",
                    m.MimeType))));

        GetProjectByIdQueryResult? project = await _unitOfWork.Projects.GetBySpecAsync(getProjectByIdSpec, cancellationToken);
        if (project is null)
        {
            _notification.AddNotification(NotificationType.ProjectNotFound, [request.ProjectId]);
            return null;
        }

        return project;
    }
}
