using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public record GetUserByTokenRecoverPasswordQuery(string Token) : IRequest<GetUserByTokenRecoverPasswordQueryResult?> { }
