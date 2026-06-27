using MediatR;

namespace CidadeInteligente.Application.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserCommandResult> { }
