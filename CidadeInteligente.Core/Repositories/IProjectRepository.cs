using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Core.Repositories;

public interface IProjectRepository {
    Task AddAsync(Project project);
    Task<PaginationResult<Project>> GetAllAsync(int page);
    Task DeleteProjectAsync(Project project);
    Task SaveChangesAsync();
    Task<Project?> GetByIdAsync(long projectId, bool tracking = false);
    Task<Project?> GetDetailsById(long projectId);
    Task<Media?> GetMediaById(long mediaId);
    void DeleteMedia(Media media);
}