using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommand(long userId, long courseId, string name, string email, Role role) : IRequest<Unit> {
    public long UserId { get; private set; } = userId;
    public long CourseId { get; private set; } = courseId;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public Role Role { get; private set; } = role;
}