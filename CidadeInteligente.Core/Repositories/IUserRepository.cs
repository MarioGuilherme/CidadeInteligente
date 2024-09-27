using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Core.Repositories;

public interface IUserRepository {
    Task AddAsync(User user);
    void Delete(User user);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email, bool tracking = false);
    Task<User?> GetByIdAsync(long userId, bool tracking = false);
    Task<User?> GetByTokenRecoverPasswordAsync(string tokenRecoverPassword);
    Task<PaginationResult<Project>> GetInvolvedAndCreatedProjectsFromUser(long userId, int page);
    Task<bool> IsEmailInUseExceptByUserId(string email, long userId = default);
    Task<bool> IsInvolvedOrCreatedProjectsAsync(long userId);
    Task<bool> UserIdExistAsync(long userId);
}