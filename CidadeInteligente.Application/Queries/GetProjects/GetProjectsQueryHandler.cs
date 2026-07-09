using CidadeInteligente.Application.Options;
using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Options;

namespace CidadeInteligente.Application.Queries.GetProjects;

public class GetProjectsQueryHandler(IUnitOfWork unitOfWork, IOptions<FileStorageOptions> azureStorageOptions, IOptions<PaginationOptions> paginationOptions) : IRequestHandler<GetProjectsQuery, GetProjectsQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _baseUrl = azureStorageOptions.Value.BaseUrl;
    private readonly int _pageSize = paginationOptions.Value.MaxPageSize;

    public async Task<GetProjectsQueryResult> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        Specification<Project, GetProjectsQueryResult.ProjectViewModel> getPagedProjectsSpec = SpecificationBuilder<Project>.Create()
            .PageBy(request.Page, _pageSize)
            .WithProjection<GetProjectsQueryResult.ProjectViewModel>(p => new(
                p.ProjectId,
                p.Title,
                p.Description,
                p.Medias.Select(m => new GetProjectsQueryResult.ProjectViewModel.MediaViewModel(m.MediaId,
                    $"{_baseUrl}/{m.FileName}",
                    m.MimeType)).ToList()))!;

        PagedResult<GetProjectsQueryResult.ProjectViewModel> pagedProjects = await _unitOfWork.Projects.GetPagedBySpecAsync(getPagedProjectsSpec, cancellationToken);
        return new(pagedProjects.Page,
            pagedProjects.TotalPages,
            pagedProjects.TotalItems,
            pagedProjects.Items);
    }
}
