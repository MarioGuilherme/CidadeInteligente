using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQuery(long areaId) : IRequest<AreaViewModel> {
    public long AreaId { get; private set; } = areaId;
}