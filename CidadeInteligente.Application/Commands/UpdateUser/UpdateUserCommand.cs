using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public record UpdateUserCommand(long UserId, long CourseId, string Name, string Email, Role Role) : IRequest<Unit> { }
