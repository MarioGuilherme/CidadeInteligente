using MediatR;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public record SendEmailRecoverCommand(string Email, string BaseUrl) : IRequest<Unit?> { }
