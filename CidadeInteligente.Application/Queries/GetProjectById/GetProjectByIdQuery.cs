using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public record GetProjectByIdQuery(int ProjectId, int? CurrentUserId = null) : IRequest<GetProjectByIdQueryResult?> { }
