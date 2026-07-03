using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Specifications.Interfaces;

namespace CidadeInteligente.Domain.Repositories;

public interface IUserRepository : ISpecificationRepository<User>
{
    void Attach(User user);
    ValueTask CreateAsync(User user);
    Task<int> DeleteByIdAsync(int userId, CancellationToken cancellationToken);
}
