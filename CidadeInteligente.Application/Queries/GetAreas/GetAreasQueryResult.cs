namespace CidadeInteligente.Application.Queries.GetAreas;

public record GetAreasQueryResult(IEnumerable<GetAreasQueryResult.AreaViewModel> Areas)
{
    public record AreaViewModel(int AreaId, string Description);
}
