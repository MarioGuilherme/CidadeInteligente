using MediatR;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommand(string newPassword, string confirmNewPassword, string token) : IRequest<Unit> {
    public string NewPassword { get; private set; } = newPassword;
    public string ConfirmNewPassword { get; private set; } = confirmNewPassword;
    public string Token { get; private set; } = token;
}