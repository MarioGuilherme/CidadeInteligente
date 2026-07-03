using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreas;

public class GetAreasQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAreasQuery, GetAreasQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreasQueryResult> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        Specification<Area, GetAreasQueryResult.AreaViewModel> spec = SpecificationBuilder<Area>.Create()
            .WithProjection(a => new GetAreasQueryResult.AreaViewModel(a.AreaId, a.Description))!;

        IEnumerable<GetAreasQueryResult.AreaViewModel> areas = await _unitOfWork.Areas.GetAllBySpecAsync(spec);
        return new(areas);
    }
}
