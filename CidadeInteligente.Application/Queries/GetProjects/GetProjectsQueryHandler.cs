using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjects;

public class GetProjectsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectsQuery, GetProjectsQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetProjectsQueryResult> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        PaginationResult<Project> paginationResult = await _unitOfWork.Projects.GetAllAsync(request.Page);

        if (request.Page != 1 && !paginationResult.Data.Any())
            paginationResult = await _unitOfWork.Projects.GetAllAsync(paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            [.. paginationResult.Data.Select(p => new GetProjectsQueryResult.ProjectViewModel(
                p.ProjectId,
                p.Title,
                p.Description,
                [.. p.Medias.Select(m => new GetProjectsQueryResult.ProjectViewModel.MediaViewModel(
                    m.MediaId,
                    m.FileName))]))]
        );
    }
}
