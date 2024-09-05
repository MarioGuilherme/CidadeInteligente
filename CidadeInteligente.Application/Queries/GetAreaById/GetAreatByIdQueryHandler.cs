using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreatByIdQueryHandler(IAreaRepository projectRepository) : IRequestHandler<GetAreaByIdQuery, Area?> {
    private readonly IAreaRepository _areaRepository = projectRepository;

    public async Task<Area?> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken) {
        Area? area = await this._areaRepository.GetByIdAsync(request.AreaId);
        return area;
    }
}