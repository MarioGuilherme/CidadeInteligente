using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public record GetUserByIdQuery(int UserId) : IRequest<GetUserByIdQueryResult?> { }
