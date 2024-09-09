using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Core.Repositories;

public interface IUserRepository {
    Task AddAsync(User user);
    Task DeleteByIdAsync(User user);
    Task<List<User>> GetAllAsync();
    Task<PaginationResult<Project>> GetInvolvedProjectsFromUser(long userId, int page);
    Task<User?> GetByIdAsync(long userId, bool tracking = false);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
}