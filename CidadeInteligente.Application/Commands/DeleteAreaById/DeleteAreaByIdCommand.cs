using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public record DeleteAreaByIdCommand(long AreaId) : IRequest<Unit> { }
