using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreatByIdQueryHandler(IAreaRepository projectRepository, IMapper mapper) : IRequestHandler<GetAreaByIdQuery, AreaViewModel?> {
    private readonly IAreaRepository _areaRepository = projectRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<AreaViewModel?> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken) {
        Area? area = await this._areaRepository.GetByIdAsync(request.AreaId);
        return this._mapper.Map<AreaViewModel?>(area);
    }
}