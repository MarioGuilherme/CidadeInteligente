using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public class ProjectRepository(CidadeInteligenteDbContext dbContext) : SpecificationRepositoryBase<Project>(dbContext), IProjectRepository
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;

    public void DeleteMedia(Media media) => _dbContext.Medias.Remove(media);
}
