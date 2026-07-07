using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class CourseRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Course>(dbContext), ICourseRepository
{
}
