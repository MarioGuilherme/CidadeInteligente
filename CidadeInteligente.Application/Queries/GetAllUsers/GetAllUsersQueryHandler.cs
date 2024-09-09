using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllUsersQuery, List<UserViewModel>> {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) {
        List<User> users = await this._userRepository.GetAllAsync();
        return this._mapper.Map<List<UserViewModel>>(users);
    }
}