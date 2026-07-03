using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjects;

public class GetProjectsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectsQuery, GetProjectsQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetProjectsQueryResult> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        int pageSize = int.Parse(Environment.GetEnvironmentVariable("Pagination:MaxPageSize")!);

        Specification<Project, GetProjectsQueryResult.ProjectViewModel> spec = SpecificationBuilder<Project>.Create()
            .PageBy(request.Page, pageSize)
            .WithProjection(p => new GetProjectsQueryResult.ProjectViewModel(
                p.ProjectId,
                p.Title,
                p.Description,
                p.Medias.Select(m => new GetProjectsQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName, m.MimeType)).ToList()))!;

        PagedResult<GetProjectsQueryResult.ProjectViewModel> pagedProjects = await _unitOfWork.Projects.GetPagedBySpecAsync(spec);
        return new(pagedProjects.Page,
            pagedProjects.TotalPages,
            pagedProjects.TotalItems,
            pagedProjects.Items);
    }
}
