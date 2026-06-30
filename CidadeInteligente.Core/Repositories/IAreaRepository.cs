using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface IAreaRepository
{
    Task AddAsync(Area area);
    void Delete(Area area);
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(int areaId, bool tracking = false);
    Task<bool> HaveProjectsAsync(int areaId);
}
