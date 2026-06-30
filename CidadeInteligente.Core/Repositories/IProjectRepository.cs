using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Core.Repositories;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    void DeleteMedia(Media media);
    void DeleteProject(Project project);
    Task<PaginationResult<Project>> GetAllAsync(int page);
    Task<Project?> GetByIdAsync(int projectId, bool tracking = false);
    Task<Project?> GetDetailsById(int projectId);
    Task<Media?> GetMediaById(int mediaId);
}
