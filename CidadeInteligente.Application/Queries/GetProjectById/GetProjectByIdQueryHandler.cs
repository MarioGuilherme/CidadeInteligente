using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetProjectByIdQueryResult> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project project = await _unitOfWork.Projects.GetDetailsById(request.ProjectId) ?? throw new ProjectNotExistException();

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
            project.InvolvedUsers.Select(iu => new GetProjectByIdQueryResult.ProjectUserViewModel(iu.UserId, iu.Name)),
            project.Medias.Select(m => new GetProjectByIdQueryResult.MediaDetailsViewModel(m.MediaId, m.Title, m.Description, m.FileName, m.Size))
        );
    }
}
