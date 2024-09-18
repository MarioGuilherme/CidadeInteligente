using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetInvolvedAndCreatedProjectsFromUser;

public class GetInvolvedAndCreatedProjectsFromUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetInvolvedAndCreatedProjectsFromUserQuery, PaginationResult<ProjectViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetInvolvedAndCreatedProjectsFromUserQuery request, CancellationToken cancellationToken) {
        if (!await this._unitOfWork.Users.UserIdExistAsync(request.UserId))
            throw new UserNotExistException();

        PaginationResult<Project> paginationResult = await this._unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, request.Page);

        if (paginationResult.Data.Count == 0)
            paginationResult = await this._unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            this._mapper.Map<List<ProjectViewModel>>(paginationResult.Data)
        );
    }
}