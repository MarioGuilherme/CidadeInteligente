using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommand(long areaId, string description) : IRequest<Unit> {
    public long AreaId { get; private set; } = areaId;
    public string Description { get; private set; } = description;
}