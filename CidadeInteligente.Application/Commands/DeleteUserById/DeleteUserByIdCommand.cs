using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public record DeleteUserByIdCommand(int UserId) : IRequest<Unit?> { }
