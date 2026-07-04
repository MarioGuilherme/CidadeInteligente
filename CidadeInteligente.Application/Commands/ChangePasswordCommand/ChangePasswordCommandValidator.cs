using FluentValidation;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("É necessário informar a nova senha!");

        RuleFor(c => c.ConfirmNewPassword)
            .NotEmpty().WithMessage("É necessário informar a confirmação da nova senha!");

        RuleFor(c => c.ConfirmNewPassword)
            .Equal(c => c.NewPassword).WithMessage("As senhas não coincidem.");

        RuleFor(c => c.Token)
            .NotEmpty().WithMessage("É necessário informar o token da redefinição da senha!")
            .Length(156).WithMessage("O token de redefinição de senha deve ter 156 caracteres!");
    }
}
