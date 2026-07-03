using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Specifications.Interfaces;

namespace CidadeInteligente.Domain.Repositories;

public interface IAreaRepository : ISpecificationRepository<Area>
{
    Task<int> DeleteByIdAsync(int areaId, CancellationToken cancellationToken);
}
