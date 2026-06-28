using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public record DeleteProjectByIdCommand(long ProjectId, long? CurrentUserId) : IRequest<Unit?> { }
