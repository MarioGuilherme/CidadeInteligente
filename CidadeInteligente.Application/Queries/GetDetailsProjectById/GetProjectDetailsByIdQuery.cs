using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetDetailsProjectById;

public class GetProjectDetailsByIdQuery(long projectId, long? userIdEditor = null) : IRequest<ProjectDetailsViewModel> {
    public long ProjectId { get; private set; } = projectId;
    public long? UserIdEditor { get; private set; } = userIdEditor;
}