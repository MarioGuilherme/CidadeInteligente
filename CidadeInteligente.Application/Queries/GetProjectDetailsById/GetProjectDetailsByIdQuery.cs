using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectDetailsById;

public record GetProjectDetailsByIdQuery(long ProjectId, long? UserIdEditor = null) : IRequest<GetProjectDetailsByIdQueryResult> { }
