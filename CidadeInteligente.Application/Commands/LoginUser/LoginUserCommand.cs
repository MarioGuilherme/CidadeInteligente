using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<LoginViewModel> {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}