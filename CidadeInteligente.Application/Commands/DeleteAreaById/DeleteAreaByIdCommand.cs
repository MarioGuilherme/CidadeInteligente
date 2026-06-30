using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public record DeleteAreaByIdCommand(int AreaId) : IRequest<Unit?> { }
