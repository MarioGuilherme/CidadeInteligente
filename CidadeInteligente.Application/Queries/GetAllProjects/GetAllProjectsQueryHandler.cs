using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper) : IRequestHandler<GetAllProjectsQuery, PaginationResult<ProjectViewModel>> {
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken) {
        PaginationResult<Project> paginationResult = await this._projectRepository.GetAllAsync(request.Page);
        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            this._mapper.Map<List<ProjectViewModel>>(paginationResult.Data)
        );
    }
}