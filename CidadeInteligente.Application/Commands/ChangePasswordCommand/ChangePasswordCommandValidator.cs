using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.NewPassword).UserPassword();
        RuleFor(c => c.ConfirmNewPassword).UserConfirmPassword(c => c.NewPassword);
        RuleFor(c => c.Token).UserToken();
    }
}
