using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : IAreaRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public Task<List<Area>> GetAllAsync() => this._dbContext.Areas.AsNoTracking().ToListAsync();

    public Task<Area?> GetByIdAsync(long areaId, bool tracking = false) => tracking
        ? this._dbContext.Areas.FirstOrDefaultAsync(a => a.AreaId == areaId)
        : this._dbContext.Areas.AsNoTracking().FirstOrDefaultAsync(a => a.AreaId == areaId);

    public async Task AddAsync(Area area) {
        await this._dbContext.Areas.AddAsync(area);
        await this._dbContext.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => this._dbContext.SaveChangesAsync();

    public async Task DeleteAreaAsync(Area area) {
        this._dbContext.Areas.Remove(area);
        await this._dbContext.SaveChangesAsync();
    }
}