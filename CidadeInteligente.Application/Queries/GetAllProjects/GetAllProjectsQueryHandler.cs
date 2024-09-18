using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllProjectsQuery, PaginationResult<ProjectViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken) {
        PaginationResult<Project> paginationResult = await this._unitOfWork.Projects.GetAllAsync(request.Page);

        if (paginationResult.Data.Count == 0)
            paginationResult = await this._unitOfWork.Projects.GetAllAsync(paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            this._mapper.Map<List<ProjectViewModel>>(paginationResult.Data)
        );
    }
}