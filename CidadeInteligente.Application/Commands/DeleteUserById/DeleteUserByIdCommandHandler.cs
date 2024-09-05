using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserByIdCommand, Unit?> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit?> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken) {
        User? user = await this._userRepository.GetByIdAsync(request.UserId);

        if (user is null) return null;

        await this._userRepository.DeleteByIdAsync(user);

        return Unit.Value;
    }
}