using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<User?> {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}