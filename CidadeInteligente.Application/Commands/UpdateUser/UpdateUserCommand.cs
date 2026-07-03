using CidadeInteligente.Domain.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public record UpdateUserCommand(int UserId, int CourseId, string Name, string Email, Role Role) : IRequest<Unit?> { }
