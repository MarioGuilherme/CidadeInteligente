using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQueryHandler(IAreaRepository areaRepository) : IRequestHandler<GetAllAreasQuery, List<Area>> {
    private readonly IAreaRepository _areaRepository = areaRepository;

    public async Task<List<Area>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken) {
        List<Area> areas = await this._areaRepository.GetAllAsync();
        return areas;
    }
}