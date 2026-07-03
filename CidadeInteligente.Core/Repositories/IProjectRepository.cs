using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications.Interfaces;

namespace CidadeInteligente.Core.Repositories;

public interface IProjectRepository : ISpecificationRepository<Project>
{
    void DeleteMedia(Media media);
}
