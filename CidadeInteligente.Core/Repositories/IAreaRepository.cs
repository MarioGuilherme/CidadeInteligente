using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface IAreaRepository {
    Task AddAsync(Area area);
    void Delete(Area area);
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(long areaId, bool tracking = false);
    Task<bool> HaveProjectsAsync(long areaId);
}