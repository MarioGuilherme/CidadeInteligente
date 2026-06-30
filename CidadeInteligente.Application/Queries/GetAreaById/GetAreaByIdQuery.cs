using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public record GetAreaByIdQuery(int AreaId) : IRequest<GetAreaByIdQueryResult?> { }
