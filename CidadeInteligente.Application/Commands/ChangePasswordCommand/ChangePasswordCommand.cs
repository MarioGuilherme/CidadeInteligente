using MediatR;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public record ChangePasswordCommand(string NewPassword, string ConfirmNewPassword, string Token) : IRequest<Unit> { }
