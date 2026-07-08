using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public record GetOrRemoveUserTokenRecoverPasswordCommand(string Token) : IRequest<GetOrRemoveUserTokenRecoverPasswordCommandResult?> { }
