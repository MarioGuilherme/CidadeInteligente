using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProjectsQuery, PaginationResult<ProjectViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken) {
        PaginationResult<Project> paginationResult = await this._unitOfWork.Projects.GetAllAsync(request.Page);

        if (paginationResult.Data.Count == 0 && request.Page != 1)
            paginationResult = await this._unitOfWork.Projects.GetAllAsync(paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            [.. paginationResult.Data.Select(p => new ProjectViewModel(
                p.ProjectId,
                p.Title,
                p.Description,
                [.. p.Medias.Select(m => new MediaViewModel(
                    m.MediaId,
                    m.FileName))]))]
        );
    }
}