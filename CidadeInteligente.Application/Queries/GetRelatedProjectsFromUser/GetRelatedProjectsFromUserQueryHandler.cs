using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Core.Common;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetRelatedProjectsFromUserQuery, GetRelatedProjectsFromUserQueryResult>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetRelatedProjectsFromUserQueryResult> Handle(GetRelatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        const int pageSize = 8;

        Specification<Project, GetRelatedProjectsFromUserQueryResult.ProjectViewModel> spec = SpecificationBuilder<Project>.Create()
            .PageBy(request.Page, pageSize)
            .AsReadOnly()
            .WithProjection(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(
                p.ProjectId,
                p.Title,
                p.Description,
                p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName)).ToList()))!;

        PagedResult<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> pagedProjects = await _unitOfWork.Projects.GetPagedBySpecAsync(spec);
        return new(pagedProjects.Page,
            pagedProjects.TotalPages,
            pagedProjects.TotalItems,
            pagedProjects.Items);


        //Specification<User> specUser = SpecificationBuilder<User>.Create()
        //    .Where(u => u.UserId == request.UserId)
        //    .WithProjection(u => new User(u.UserId))
        //    .NoTracking()
        //    .Build();

        //User? user = await _unitOfWork.Users.GetProjectionBySpecAsync(specUser);
        //if (user is null)
        //{
        //    Log.Warning("User with ID {UserId} not found.", request.UserId);
        //    _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
        //    return null;
        //}

        //const int pageSize = 8;

        //Specification<Project> countSpec = SpecificationBuilder<Project>.Create().Build();
        //int totalCount = await _unitOfWork.Projects.CountBySpecAsync(countSpec);
        //int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        //int page = request.Page < 1 ? 1 : request.Page;
        //if (totalPages > 0 && page > totalPages)
        //    page = totalPages;

        //Specification<Project> specProject = SpecificationBuilder<Project>.Create()
        //    .WithProjection(p => new Project(p.ProjectId,
        //        p.Title,
        //        p.Description,
        //        p.Medias.Select(m => new Media(m.FileName)).First()))
        //    .PageBy(page, pageSize)
        //    .AsReadOnly()
        //    .Build();
        //PagedResult<Project> pagedProjects = await _unitOfWork.Projects.GetPagedProjectionAsync(specProject);

        //return new(pagedProjects.Page,
        //    pagedProjects.TotalPages,
        //    pagedProjects.TotalItems,
        //    [.. pagedProjects.Items.Select(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(p.ProjectId,
        //        p.Title,
        //        p.Description,
        //        [.. p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName))]))]);










        //.AsNoTracking()
        //.Include(u => u.InvolvedProjects)
        //.ThenInclude(p => p.Medias)
        //.Include(u => u.CreatedProjects)
        //.ThenInclude(p => p.Medias)
        //.FirstAsync(u => u.UserId == userId);

        //return user.CreatedProjects.Concat(user.InvolvedProjects).DistinctBy(p => p.ProjectId).GetPaged(page);

        //PagedResult<Project> pagedProjects = await _unitOfWork.Projects.GetPagedProjectionAsync(spec);
        //return new(paginationResult.CurrentPage,
        //    paginationResult.TotalPages,
        //    paginationResult.ItemsCount,
        //    [.. paginationResult.Data.Select(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(p.ProjectId,
        //        p.Title,
        //        p.Description,
        //        [.. p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName))]))]);






        //PagedResult<Project> paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, request.Page);
    }
}