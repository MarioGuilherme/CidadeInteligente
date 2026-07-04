using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Area>(dbContext), IAreaRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private readonly DbSet<Area> _dbSet = dbContext.Set<Area>();

    public Task<int> DeleteByIdAsync(int areaId, CancellationToken cancellationToken) => _dbContext.Areas
        .Where(a => a.AreaId == areaId)
        .ExecuteDeleteAsync(cancellationToken);
}
