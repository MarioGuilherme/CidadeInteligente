using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommand(long projectId) : IRequest<Unit> {
    public long ProjectId { get; private set; } = projectId;
    public long? UserIdEditor { get; set; }
}