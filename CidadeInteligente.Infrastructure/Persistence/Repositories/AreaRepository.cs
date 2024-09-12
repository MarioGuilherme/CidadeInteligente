using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : IAreaRepository {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public async Task AddAsync(Area area) {
        await this._dbContext.Areas.AddAsync(area);
    }

    public void Delete(Area area) => this._dbContext.Areas.Remove(area);

    public Task<List<Area>> GetAllAsync() => this._dbContext.Areas.AsNoTracking().ToListAsync();

    public Task<Area?> GetByIdAsync(long areaId, bool tracking = false) => tracking
        ? this._dbContext.Areas.FirstOrDefaultAsync(a => a.AreaId == areaId)
        : this._dbContext.Areas.AsNoTracking().FirstOrDefaultAsync(a => a.AreaId == areaId);

    public async Task<bool> HaveProjectsAsync(long areaId) => (await this._dbContext.Areas
        .Include(a => a.Projects)
        .AsNoTracking()
        .FirstAsync(a => a.AreaId == areaId)).Projects.Count > 0;
}