using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IProjectRepository projectRepository) : IRequestHandler<GetAllProjectsQuery, List<ProjectViewModel>> {
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<List<ProjectViewModel>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken) {
        List<Project> projects = await this._projectRepository.GetAllAsync();
        List<ProjectViewModel> projectViewModels = projects
            .Select(p => new ProjectViewModel(
                p.CreatorUserId,
                p.ProjectId,
                p.AreaId,
                p.CourseId,
                p.Title,
                p.Description,
                p.RegisteredAt,
                p.StartedAt,
                p.FinishedAt,
                p.Medias.ToList()
            )).ToList();
        return projectViewModels;
    }
}