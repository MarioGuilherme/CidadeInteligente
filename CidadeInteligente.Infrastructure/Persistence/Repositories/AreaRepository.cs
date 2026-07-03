using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Area>(dbContext), IAreaRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private readonly DbSet<Area> _dbSet = dbContext.Set<Area>();
}
