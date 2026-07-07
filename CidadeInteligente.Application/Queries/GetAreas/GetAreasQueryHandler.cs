using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreas;

public class GetAreasQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAreasQuery, GetAreasQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreasQueryResult> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        Specification<Area, GetAreasQueryResult.AreaViewModel> getAreasSpec = SpecificationBuilder<Area>.Create()
            .WithProjection<GetAreasQueryResult.AreaViewModel>(a => new(a.AreaId, a.Description))!;

        IEnumerable<GetAreasQueryResult.AreaViewModel> areas = await _unitOfWork.Areas.GetAllBySpecAsync(getAreasSpec, cancellationToken);
        return new(areas);
    }
}
