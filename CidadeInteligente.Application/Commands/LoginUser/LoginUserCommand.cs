using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommand(string email, string password) : IRequest<LoginViewModel> {
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
}