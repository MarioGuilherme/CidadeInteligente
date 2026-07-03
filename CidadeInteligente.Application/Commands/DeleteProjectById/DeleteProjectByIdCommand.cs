using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public record DeleteProjectByIdCommand(int ProjectId, int CurrentUserId) : IRequest<Unit?> { }
