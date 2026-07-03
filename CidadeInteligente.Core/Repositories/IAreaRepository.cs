using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications.Interfaces;

namespace CidadeInteligente.Core.Repositories;

public interface IAreaRepository : ISpecificationRepository<Area>
{
    Task<int> DeleteByIdAsync(int areaId, CancellationToken cancellationToken);
}
