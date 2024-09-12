using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface ICourseRepository {
    Task AddAsync(Course course);
    void Delete(Course course);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(long courseId, bool tracking = false);
    Task<bool> HaveProjectsAsync(long courseId);
}