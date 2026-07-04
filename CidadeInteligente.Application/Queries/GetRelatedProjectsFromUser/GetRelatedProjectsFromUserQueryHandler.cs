using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetRelatedProjectsFromUserQuery, GetRelatedProjectsFromUserQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetRelatedProjectsFromUserQueryResult?> Handle(GetRelatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        Specification<User> specUser = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId == request.UserId)
            .Build();

        if (!await _unitOfWork.Users.AnyBySpecAsync(specUser))
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        int pageSize = int.Parse(Environment.GetEnvironmentVariable("Pagination:MaxPageSize")!);
        Specification<Project, GetRelatedProjectsFromUserQueryResult.ProjectViewModel> spec = SpecificationBuilder<Project>.Create()
            .PageBy(request.Page, pageSize)
            .WithProjection(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(
                p.ProjectId,
                p.Title,
                p.Description,
                p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName, m.MimeType)).ToList()))!;

        PagedResult<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> pagedProjects = await _unitOfWork.Projects.GetPagedBySpecAsync(spec);
        return new(pagedProjects.Page,
            pagedProjects.TotalPages,
            pagedProjects.TotalItems,
            pagedProjects.Items);
    }
}
