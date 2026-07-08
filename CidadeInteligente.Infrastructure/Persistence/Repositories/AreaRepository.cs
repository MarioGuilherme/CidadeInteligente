using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Area>(dbContext), IAreaRepository
{
}
