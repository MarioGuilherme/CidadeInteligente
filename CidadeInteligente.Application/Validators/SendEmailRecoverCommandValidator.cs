using CidadeInteligente.Application.Commands.SendEmailRecover;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class SendEmailRecoverCommandValidator : AbstractValidator<SendEmailRecoverCommand> {
    public SendEmailRecoverCommandValidator() {
        this.RuleFor(s => s.Email)
            .NotEmpty().WithMessage("É necessário informar o e-mail do usuário!")
            .EmailAddress().WithMessage("Informe um e-mail válido!")
            .MaximumLength(60).WithMessage("O e-mail do usuário não pode exceder 60 caracteres!");
    }
}