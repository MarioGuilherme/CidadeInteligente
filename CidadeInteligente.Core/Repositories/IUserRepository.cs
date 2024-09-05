using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface IUserRepository {
    Task AddAsync(User user);
    Task DeleteByIdAsync(User user);
    Task<List<User>> GetAllAsync();
    Task<List<Project>> GetCreatedProjectsFromUser(long userId);
    Task<User?> GetByIdAsync(long userId, bool tracking = false);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
}