using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreas;

public class GetAreasQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAreasQuery, GetAreasQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreasQueryResult> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        List<Area> areas = await _unitOfWork.Areas.GetAllAsync();

        return new(areas.Select(a => new GetAreasQueryResult.AreaViewModel(a.AreaId, a.Description)));
    }
}
