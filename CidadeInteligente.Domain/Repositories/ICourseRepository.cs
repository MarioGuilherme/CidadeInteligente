using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Specifications.Interfaces;

namespace CidadeInteligente.Domain.Repositories;

public interface ICourseRepository : ISpecificationRepository<Course>
{
    Task<int> DeleteByIdAsync(int areaId, CancellationToken cancellationToken);
}
