using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Specifications.Interfaces;

namespace CidadeInteligente.Domain.Repositories;

public interface IProjectRepository : ISpecificationRepository<Project>
{
    void DeleteMedia(Media media);
}
