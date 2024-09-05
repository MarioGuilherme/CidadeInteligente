using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class UserRepository (CidadeInteligenteDbContext dbContext) : IUserRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public async Task AddAsync(User user) {
        await this._dbContext.Users.AddAsync(user);
        await this._dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(User user) {
        this._dbContext.Users.Remove(user);
        await this._dbContext.SaveChangesAsync();
    }

    public Task<List<User>> GetAllAsync() => this._dbContext.Users
        .Include(u => u.Course)
        .AsNoTracking()
        .ToListAsync();

    public async Task<List<Project>> GetCreatedProjectsFromUser(long userId) => (await this._dbContext.Users
        .Include(u => u.CreatedProjects)
        .ThenInclude(a => a.Medias)
        .AsNoTracking()
        .FirstAsync(u => u.UserId == userId)).CreatedProjects;

    public Task<User?> GetByIdAsync(long userId, bool tracking = false) => tracking
        ? this._dbContext.Users.FirstOrDefaultAsync(c => c.UserId == userId)
        : this._dbContext.Users.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId);

    public Task<User?> GetByEmailAsync(string email) => this._dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task SaveChangesAsync() => this._dbContext.SaveChangesAsync();
}