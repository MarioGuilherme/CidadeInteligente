using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQueryHandler(IAreaRepository areaRepository, IMapper mapper) : IRequestHandler<GetAllAreasQuery, List<AreaViewModel>> {
    private readonly IAreaRepository _areaRepository = areaRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<AreaViewModel>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken) {
        List<Area> areas = await this._areaRepository.GetAllAsync();
        return this._mapper.Map<List<AreaViewModel>>(areas);
    }
}