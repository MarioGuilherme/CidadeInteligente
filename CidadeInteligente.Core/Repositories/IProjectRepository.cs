using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface IProjectRepository {
    Task AddAsync(Project project);
    Task<List<Project>> GetAllAsync();
    Task DeleteProjectAsync(Project project);
    Task SaveChangesAsync();
    Task<Project?> GetByIdAsync(long projectId, bool tracking = false);
    Task<Media?> GetMediaById(long mediaId);
    void DeleteMedia(Media media);
}