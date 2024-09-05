using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<long> {
    public long CourseId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}