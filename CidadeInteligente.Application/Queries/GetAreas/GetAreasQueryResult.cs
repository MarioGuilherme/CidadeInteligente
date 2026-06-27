namespace CidadeInteligente.Application.Queries.GetAreas;

public record GetAreasQueryResult(IEnumerable<GetAreasQueryResult.AreaViewModel> Areas)
{
    public record AreaViewModel(long AreaId, string Description);
}
