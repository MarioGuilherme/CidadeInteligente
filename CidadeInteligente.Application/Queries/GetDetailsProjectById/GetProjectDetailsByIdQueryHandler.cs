using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetDetailsProjectById;

public class GetProjectDetailsByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectDetailsByIdQuery, ProjectDetailsViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ProjectDetailsViewModel> Handle(GetProjectDetailsByIdQuery request, CancellationToken cancellationToken) {
        Project project = await this._unitOfWork.Projects.GetDetailsById(request.ProjectId) ?? throw new ProjectNotExistException();

        if (request.UserIdEditor is not null && !(request.UserIdEditor == project.CreatorUserId || project.InvolvedUsers.Any(iu => iu.UserId == request.UserIdEditor)))
            throw new UserIsReadOnlyException();

        return new(project.ProjectId,
            project.Title,
            project.Area.Description,
            project.AreaId,
            project.Course.Description,
            project.CourseId,
            project.Description!,
            project.StartedAt,
            project.FinishedAt,
            [.. project.InvolvedUsers.Select(iu => new ProjectUserViewModel(iu.UserId, iu.Name))],
            [.. project.Medias.Select(m => new MediaDetailsViewModel(m.MediaId, m.Title, m.Description, m.FileName, m.Size))]
        );
    }
}