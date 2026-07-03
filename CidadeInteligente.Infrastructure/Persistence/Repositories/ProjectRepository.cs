using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class ProjectRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Project>(dbContext), IProjectRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private readonly DbSet<Project> _dbSet = dbContext.Set<Project>();

    public void DeleteMedia(Media media) => _dbContext.Medias.Remove(media);
}
