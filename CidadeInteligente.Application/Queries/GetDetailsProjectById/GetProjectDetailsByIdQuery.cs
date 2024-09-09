using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetDetailsProjectById;

public class GetProjectDetailsByIdQuery(long projectId) : IRequest<ProjectDetailsViewModel?> {
    public long ProjectId { get; private set; } = projectId;
}