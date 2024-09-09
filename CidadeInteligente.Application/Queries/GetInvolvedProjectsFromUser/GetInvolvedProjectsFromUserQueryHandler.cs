using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetInvolvedProjectsFromUser;

public class GetInvolvedProjectsFromUserQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetInvolvedProjectsFromUserQuery, PaginationResult<ProjectViewModel>> {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginationResult<ProjectViewModel>> Handle(GetInvolvedProjectsFromUserQuery request, CancellationToken cancellationToken) {
        PaginationResult<Project> paginationResult = await this._userRepository.GetInvolvedProjectsFromUser(request.UserId, request.Page);
        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            this._mapper.Map<List<ProjectViewModel>>(paginationResult.Data)
        );
    }
}