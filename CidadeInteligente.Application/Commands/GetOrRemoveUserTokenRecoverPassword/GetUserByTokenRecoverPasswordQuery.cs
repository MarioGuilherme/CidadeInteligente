using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public record GetUserByTokenRecoverPasswordQuery(string Token) : IRequest<GetUserByTokenRecoverPasswordQueryResult?> { }
