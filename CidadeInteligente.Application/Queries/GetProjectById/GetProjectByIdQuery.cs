using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQuery(long projectId) : IRequest<Project?> {
    public long ProjectId { get; private set; } = projectId;
}