using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public record DeleteUserByIdCommand(long UserId) : IRequest<Unit?> { }
