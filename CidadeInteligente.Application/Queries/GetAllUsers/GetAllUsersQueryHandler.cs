using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllUsersQuery, List<UserViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) {
        List<User> users = await this._unitOfWork.Users.GetAllAsync();
        return this._mapper.Map<List<UserViewModel>>(users);
    }
}