using MediatR;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommand(string email) : IRequest<Unit> {
    public string Email { get; private set; } = email;
}