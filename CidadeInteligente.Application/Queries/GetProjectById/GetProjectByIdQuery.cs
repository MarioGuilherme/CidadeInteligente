using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public record GetProjectByIdQuery(long ProjectId, long? CurrentUserId = null) : IRequest<GetProjectByIdQueryResult?> { }
