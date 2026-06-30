using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public record UpdateAreaCommand(int AreaId, string Description) : IRequest<Unit?> { }
