using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaByAreaId;

public class GetAreaByIdQuery(long areaId) : IRequest<Area?> {
    public long AreaId { get; private set; } = areaId;
}