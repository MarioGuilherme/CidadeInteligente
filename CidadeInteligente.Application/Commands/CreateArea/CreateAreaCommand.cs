using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public record CreateAreaCommand(string Description) : IRequest<long> { }
