using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreas;

public record GetAreasQuery : IRequest<GetAreasQueryResult> { }
