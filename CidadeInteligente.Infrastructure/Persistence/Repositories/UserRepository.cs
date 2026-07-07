using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class UserRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<User>(dbContext), IUserRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public async ValueTask CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
}
