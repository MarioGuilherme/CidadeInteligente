using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommand(long courseId, string name, string email, string password, Role role) : IRequest<long> {
    public long CourseId { get; private set; } = courseId;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public Role Role { get; private set; } = role;
}