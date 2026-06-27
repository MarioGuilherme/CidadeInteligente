using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetInvolvedAndCreatedProjectsFromUser;

public class GetInvolvedAndCreatedProjectsFromUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetInvolvedAndCreatedProjectsFromUserQuery, PaginationResult<ProjectViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetInvolvedAndCreatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.UserIdExistAsync(request.UserId))
            throw new UserNotExistException();

        PaginationResult<Project> paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, request.Page);

        if (paginationResult.Data.Count == 0)
            paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            [.. paginationResult.Data.Select(p => new ProjectViewModel(p.ProjectId,
                p.Title,
                p.Description,
                [.. p.Medias.Select(m => new MediaViewModel(m.MediaId, m.FileName))]))]
        );
    }
}