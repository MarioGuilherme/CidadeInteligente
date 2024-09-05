using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Core.Repositories;

public interface IAreaRepository {
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(long areaId, bool tracking = false);
    Task AddAsync(Area area);
    Task SaveChangesAsync();
    Task DeleteAreaAsync(Area area);
}