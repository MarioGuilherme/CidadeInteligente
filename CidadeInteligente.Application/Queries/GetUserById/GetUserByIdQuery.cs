using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public record GetUserByIdQuery(long UserId) : IRequest<GetUserByIdQueryResult> { }
