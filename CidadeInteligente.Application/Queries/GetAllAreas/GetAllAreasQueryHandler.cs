using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllAreasQuery, List<AreaViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<AreaViewModel>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken) {
        List<Area> areas = await this._unitOfWork.Areas.GetAllAsync();
        return this._mapper.Map<List<AreaViewModel>>(areas);
    }
}