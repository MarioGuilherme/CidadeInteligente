using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public record GetUserByTokenRecoverPasswordCommand(string Token) : IRequest<GetUserByTokenRecoverPasswordCommandResult?> { }
