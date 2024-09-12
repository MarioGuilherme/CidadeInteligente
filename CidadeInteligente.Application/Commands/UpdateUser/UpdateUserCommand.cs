using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Unit> {
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; }
}