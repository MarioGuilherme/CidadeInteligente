using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface ICourseRepository
{
    Task AddAsync(Course course);
    void Delete(Course course);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int courseId, bool tracking = false);
    Task<bool> HaveProjectsAsync(int courseId);
}
