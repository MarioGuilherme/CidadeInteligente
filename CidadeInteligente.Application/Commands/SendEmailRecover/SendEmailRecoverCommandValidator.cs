using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandValidator : AbstractValidator<SendEmailRecoverCommand>
{
    public SendEmailRecoverCommandValidator() => RuleFor(c => c.Email).UserEmail();
}
