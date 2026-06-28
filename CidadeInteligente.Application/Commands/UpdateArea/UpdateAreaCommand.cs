using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public record UpdateAreaCommand(long AreaId, string Description) : IRequest<Unit?> { }
