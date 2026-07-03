using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class CourseRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Course>(dbContext), ICourseRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private readonly DbSet<Course> _dbSet = dbContext.Set<Course>();
}
