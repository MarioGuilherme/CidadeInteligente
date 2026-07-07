using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Repositories;

public interface IUserRepository : ISpecificationRepository<User>
{
    ValueTask CreateAsync(User user, CancellationToken cancellationToken = default);
}
