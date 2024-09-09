using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class ProjectRepository(CidadeInteligenteDbContext dbContext) : IProjectRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public async Task AddAsync(Project project) {
        await this._dbContext.Projects.AddAsync(project);
        this._dbContext.Users.AttachRange(project.InvolvedUsers);
        await this._dbContext.SaveChangesAsync();
    }

    public Task<PaginationResult<Project>> GetAllAsync(int page) => this._dbContext.Projects
        .Include(p => p.Medias)
        .AsNoTracking()
        .GetPaged(page);

    public async Task DeleteProjectAsync(Project project) {
        this._dbContext.Medias.RemoveRange(project.Medias);
        this._dbContext.Projects.Remove(project);
        await this._dbContext.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => this._dbContext.SaveChangesAsync();

    public async Task<Project?> GetByIdAsync(long projectId, bool tracking = false) => tracking
        ? await this._dbContext.Projects.Include(p => p.Medias)
                                        .Include(p => p.InvolvedUsers)
                                        .FirstOrDefaultAsync(p => p.ProjectId == projectId)
        : await this._dbContext.Projects.Include(p => p.Medias)
                                        .Include(p => p.InvolvedUsers)
                                        .AsNoTracking().FirstOrDefaultAsync(p => p.ProjectId == projectId);

    public Task<Project?> GetDetailsById(long projectId) => this._dbContext.Projects
        .Include(p => p.Area)
        .Include(p => p.Course)
        .Include(p => p.Medias)
        .Include(p => p.InvolvedUsers)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.ProjectId == projectId);

    public Task<Media?> GetMediaById(long mediaId) => this._dbContext.Medias.FirstOrDefaultAsync(m => m.MediaId == mediaId);

    public void DeleteMedia(Media media) => this._dbContext.Medias.Remove(media);
}