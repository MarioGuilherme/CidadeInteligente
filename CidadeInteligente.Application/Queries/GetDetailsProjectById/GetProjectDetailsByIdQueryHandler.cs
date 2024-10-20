using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetDetailsProjectById;

public class GetProjectDetailsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetProjectDetailsByIdQuery, ProjectDetailsViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ProjectDetailsViewModel> Handle(GetProjectDetailsByIdQuery request, CancellationToken cancellationToken) {
        Project project = await this._unitOfWork.Projects.GetDetailsById(request.ProjectId) ?? throw new ProjectNotExistException();

        if (request.UserIdEditor is not null && !(request.UserIdEditor == project.CreatorUserId || project.InvolvedUsers.Any(iu => iu.UserId == request.UserIdEditor)))
            throw new UserIsReadOnlyException();

        return this._mapper.Map<ProjectDetailsViewModel>(project);
    }
}