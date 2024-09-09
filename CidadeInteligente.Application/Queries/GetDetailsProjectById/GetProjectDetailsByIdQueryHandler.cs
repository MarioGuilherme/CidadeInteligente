using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetDetailsProjectById;

public class GetProjectDetailsByIdQueryHandler(IProjectRepository courseRepository, IMapper mapper) : IRequestHandler<GetProjectDetailsByIdQuery, ProjectDetailsViewModel?> {
    private readonly IProjectRepository _projectRepository = courseRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ProjectDetailsViewModel?> Handle(GetProjectDetailsByIdQuery request, CancellationToken cancellationToken) {
        Project? project = await this._projectRepository.GetDetailsById(request.ProjectId);
        return this._mapper.Map<ProjectDetailsViewModel>(project);
    }
}