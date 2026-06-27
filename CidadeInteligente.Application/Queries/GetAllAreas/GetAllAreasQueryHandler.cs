using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllAreasQuery, List<AreaViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<AreaViewModel>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
    {
        List<Area> areas = await _unitOfWork.Areas.GetAllAsync();
        return [.. areas.Select(a => new AreaViewModel(a.AreaId, a.Description))];
    }
}