using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public record GetProjectByIdQuery(long ProjectId, long? UserIdEditor = null) : IRequest<GetProjectByIdQueryResult> { }
