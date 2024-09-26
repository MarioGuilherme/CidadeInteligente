using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand> {
    public ChangePasswordCommandValidator() {
        this.RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("É necessário informar a nova senha!");

        this.RuleFor(c => c.ConfirmNewPassword)
            .NotEmpty().WithMessage("É necessário informar a confirmação da nova senha!");

        this.RuleFor(c => c.ConfirmNewPassword)
            .Equal(c => c.NewPassword).WithMessage("As senhas não coincidem.");

        this.RuleFor(c => c.Token)
            .NotEmpty().WithMessage("É necessário informar o token da redefinição da senha!")
            .Length(156).WithMessage("O token de redefinição de senha deve ter 156 caracteres!");
    }
}