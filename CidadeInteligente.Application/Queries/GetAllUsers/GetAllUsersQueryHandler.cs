using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, List<User>> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) {
        List<User> users = await this._userRepository.GetAllAsync();
        return users;
    }
}