using CidadeInteligente.Application.Options;
using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Projects;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryHandler(INotificationContext notification,
    IUnitOfWork unitOfWork,
    IOptions<FileStorageOptions> fileStorageOptions,
    IOptions<PaginationOptions> paginationOptions) : IRequestHandler<GetRelatedProjectsFromUserQuery, GetRelatedProjectsFromUserQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _baseUrl = fileStorageOptions.Value.BaseUrl;
    private readonly int _pageSize = paginationOptions.Value.MaxPageSize;

    public async Task<GetRelatedProjectsFromUserQueryResult?> Handle(GetRelatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByIdSpec = UserSpecifications.GetById(request.UserId).Build();
        if (!await _unitOfWork.Users.AnyBySpecAsync(getUserByIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        Specification<Project, GetRelatedProjectsFromUserQueryResult.ProjectViewModel> getRelatedProjectsFromUserSpec = ProjectSpecifications.GetRelatedProjectsFromUser(request.UserId)
            .PageBy(request.Page, _pageSize)
            .WithProjection<GetRelatedProjectsFromUserQueryResult.ProjectViewModel>(p => new(
                p.ProjectId,
                p.Title,
                p.Description,
                p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(
                    m.MediaId,
                    $"{_baseUrl}/{m.FileName}",
                    m.MimeType)).ToList()))!;

        PagedResult<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> pagedProjects = await _unitOfWork.Projects.GetPagedBySpecAsync(getRelatedProjectsFromUserSpec, cancellationToken);
        return new(pagedProjects.Page,
            pagedProjects.TotalPages,
            pagedProjects.TotalItems,
            pagedProjects.Items);
    }
}
