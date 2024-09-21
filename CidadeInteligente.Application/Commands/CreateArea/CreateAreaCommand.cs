using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommand(string description) : IRequest<long> {
    public string Description { get; private set; } = description;
}