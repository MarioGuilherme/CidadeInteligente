using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface ICourseRepository {
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(long courseId, bool tracking = false);
    Task AddAsync(Course course);
    Task SaveChangesAsync();
    Task DeleteByIdAsync(Course course);
}