using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Core.Repositories;

public interface IProjectRepository {
    Task AddAsync(Project project);
    void DeleteMedia(Media media);
    void DeleteProject(Project project);
    Task<PaginationResult<Project>> GetAllAsync(int page);
    Task<Project?> GetByIdAsync(long projectId, bool tracking = false);
    Task<Project?> GetDetailsById(long projectId);
    Task<Media?> GetMediaById(long mediaId);
}