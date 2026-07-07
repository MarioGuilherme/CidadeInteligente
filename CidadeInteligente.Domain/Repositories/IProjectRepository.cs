using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Repositories;

public interface IProjectRepository : ISpecificationRepository<Project>
{
    void DeleteMedia(Media media);
}
