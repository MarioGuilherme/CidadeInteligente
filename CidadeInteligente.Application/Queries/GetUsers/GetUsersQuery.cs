using MediatR;

namespace CidadeInteligente.Application.Queries.GetUsers;

public record GetUsersQuery : IRequest<GetUsersQueryResult> { }
