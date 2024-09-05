using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;
internal class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User?> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        User? user = await this._userRepository.GetByIdAsync(request.UserId);
        return user;
    }
}
