using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteUserByIdCommand(long userId) : IRequest<Unit?> {
    public long UserId { get; private set; } = userId;
}