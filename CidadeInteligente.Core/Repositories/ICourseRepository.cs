using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications.Interfaces;

namespace CidadeInteligente.Core.Repositories;

public interface ICourseRepository : ISpecificationRepository<Course>
{
    Task<int> DeleteByIdAsync(int areaId, CancellationToken cancellationToken);
}
