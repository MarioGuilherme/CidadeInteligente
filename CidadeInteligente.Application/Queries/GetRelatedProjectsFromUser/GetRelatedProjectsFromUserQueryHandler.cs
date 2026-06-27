using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRelatedProjectsFromUserQuery, GetRelatedProjectsFromUserQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetRelatedProjectsFromUserQueryResult> Handle(GetRelatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.UserIdExistAsync(request.UserId))
            throw new UserNotExistException();

        PaginationResult<Project> paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, request.Page);

        if (!paginationResult.Data.Any())
            paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            [.. paginationResult.Data.Select(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(p.ProjectId,
                p.Title,
                p.Description,
                [.. p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName))]))]
        );
    }
}