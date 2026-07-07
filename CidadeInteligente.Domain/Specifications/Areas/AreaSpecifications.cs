using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Specifications.Areas;

public static class AreaSpecifications
{
    public static SpecificationBuilder<Area> GetById(int areaId)
    {
        return SpecificationBuilder<Area>.Create().Where(a => a.AreaId == areaId);
    }
}
