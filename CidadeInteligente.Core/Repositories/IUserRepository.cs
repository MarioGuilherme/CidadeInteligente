using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications.Interfaces;

namespace CidadeInteligente.Core.Repositories;

public interface IUserRepository : ISpecificationRepository<User>
{
    void Attach(User user);
    ValueTask CreateAsync(User user);
    Task<int> DeleteByIdAsync(int userId, CancellationToken cancellationToken);
}
