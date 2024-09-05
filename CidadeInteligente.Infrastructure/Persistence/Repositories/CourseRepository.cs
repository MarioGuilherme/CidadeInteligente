using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class CourseRepository(CidadeInteligenteDbContext dbContext) : ICourseRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public Task<List<Course>> GetAllAsync() => this._dbContext.Courses.AsNoTracking().ToListAsync();

    public Task<Course?> GetByIdAsync(long courseId, bool tracking = false) => tracking
        ? this._dbContext.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId)
        : this._dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.CourseId == courseId);

    public async Task AddAsync(Course course) {
        await this._dbContext.Courses.AddAsync(course);
        await this._dbContext.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => this._dbContext.SaveChangesAsync();

    public async Task DeleteByIdAsync(Course course) {
        this._dbContext.Courses.Remove(course);
        await this._dbContext.SaveChangesAsync();
    }
}