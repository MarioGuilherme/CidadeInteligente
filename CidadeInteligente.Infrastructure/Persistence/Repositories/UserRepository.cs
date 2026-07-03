using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class UserRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<User>(dbContext), IUserRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public void Attach(User user) => _dbContext.Users.Attach(user);

    public async ValueTask CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public Task<int> DeleteByIdAsync(int userId, CancellationToken cancellationToken) => _dbContext.Users
        .Where(u => u.UserId == userId)
        .ExecuteDeleteAsync(cancellationToken);
}
