using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommand(string description) : IRequest<Unit> {
    public long AreaId { get; set; }
    public string Description { get; private set; } = description;
}