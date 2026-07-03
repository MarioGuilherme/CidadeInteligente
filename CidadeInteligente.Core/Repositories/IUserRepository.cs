using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications.Interfaces;

namespace CidadeInteligente.Core.Repositories;

public interface IUserRepository : ISpecificationRepository<User>
{
    ValueTask CreateAsync(User user);
    Task<int> DeleteByIdAsync(int userId, CancellationToken cancellationToken);
}
