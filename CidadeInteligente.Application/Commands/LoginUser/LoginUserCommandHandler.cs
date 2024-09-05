using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginUserCommand, User?> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User?> Handle(LoginUserCommand request, CancellationToken cancellationToken) {
        User? possibleUser = await this._userRepository.GetByEmailAsync(request.Email);

        if (possibleUser is null) return null;

        if (!Verify(request.Password, possibleUser.Password)) return null;

        return possibleUser;
    }
}