using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommand : IRequest<Unit> {
    public long AreaId { get; set; }
    public string Description { get; set; } = null!;
}