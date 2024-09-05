using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectRepository courseRepository) : IRequestHandler<GetProjectByIdQuery, Project?> {
    private readonly IProjectRepository _courseRepository = courseRepository;

    public async Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken) {
        Project? course = await this._courseRepository.GetByIdAsync(request.ProjectId);
        return course;
    }
}
