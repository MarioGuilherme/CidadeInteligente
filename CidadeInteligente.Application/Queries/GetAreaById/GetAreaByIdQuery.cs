using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public record GetAreaByIdQuery(long AreaId) : IRequest<GetAreaByIdQueryResult> { }
