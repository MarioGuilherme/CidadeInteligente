using MediatR;

namespace CidadeInteligente.Application.Queries.AuthenticateUser;

public record AuthenticateUserQuery(string Email, string Password) : IRequest<AuthenticateUserQueryResult?> { }
