using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommand : IRequest<long> {
    public string Description { get; set; } = null!;
}