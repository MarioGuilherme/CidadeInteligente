using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class UserRepository(CidadeInteligenteDbContext dbContext) : IUserRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public async Task AddAsync(User user) {
        await this._dbContext.Users.AddAsync(user);
    }

    public void Delete(User user) => this._dbContext.Users.Remove(user);

    public Task<List<User>> GetAllAsync() => this._dbContext.Users
        .Include(u => u.Course)
        .AsNoTracking()
        .ToListAsync();

    public Task<User?> GetByEmailAsync(string email, bool tracking = false) => tracking
        ? this._dbContext.Users.FirstOrDefaultAsync(u => u.Email == email)
        : this._dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByIdAsync(long userId, bool tracking = false) => tracking
        ? this._dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId)
        : this._dbContext.Users.Include(u => u.Course).AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);

    public Task<User?> GetByTokenRecoverPasswordAsync(string tokenRecoverPassword) => this._dbContext.Users.FirstOrDefaultAsync(u => u.TokenRecoverPassword == tokenRecoverPassword);

    public async Task<PaginationResult<Project>> GetInvolvedAndCreatedProjectsFromUser(long userId, int page) {
        User user = await this._dbContext.Users
            .Include(u => u.InvolvedProjects)
            .ThenInclude(p => p.Medias)
            .Include(u => u.CreatedProjects)
            .ThenInclude(p => p.Medias)
            .AsNoTracking()
            .FirstAsync(u => u.UserId == userId);

        return user.CreatedProjects.Concat(user.InvolvedProjects).Distinct().GetPaged(page);
    }

    public Task<bool> IsEmailInUse(string email) => this._dbContext.Users.AnyAsync(u => u.Email == email);

    public async Task<bool> IsInvolvedOrCreatedProjectsAsync(long userId) {
        User user = await this._dbContext.Users
            .Include(u => u.InvolvedProjects)
            .Include(u => u.CreatedProjects)
            .AsNoTracking()
            .FirstAsync(u => u.UserId == userId);

        return user.InvolvedProjects.Count != 0 || user.InvolvedProjects.Count != 0;
    }

    public Task<bool> UserIdExistAsync(long userId) => this._dbContext.Users.AnyAsync(u => u.UserId == userId);
}