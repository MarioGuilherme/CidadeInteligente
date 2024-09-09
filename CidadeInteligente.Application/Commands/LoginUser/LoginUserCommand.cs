using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<UserViewModel?> {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}