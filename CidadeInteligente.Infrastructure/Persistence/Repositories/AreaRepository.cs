using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class AreaRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Area>(dbContext), IAreaRepository
{
}
