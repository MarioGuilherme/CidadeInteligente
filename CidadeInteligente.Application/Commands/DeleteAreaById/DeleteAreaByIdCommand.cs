using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommand(long areaId) : IRequest<Unit?> {
    public long AreaId { get; private set; } = areaId;
}