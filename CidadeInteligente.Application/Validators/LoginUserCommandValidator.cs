using CidadeInteligente.Application.Commands.LoginUser;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand> {
    public LoginUserCommandValidator() {
        this.RuleFor(u => u.Email)
            .NotNull().WithMessage("É necessário informar o e-mail do usuário!")
            .NotEmpty().WithMessage("O e-mail do usuário não pode estar em branco!")
            .EmailAddress().WithMessage("Informe um e-mail válido!")
            .MaximumLength(60).WithMessage("O e-mail do usuário não pode exceder 60 caracteres!");

        this.RuleFor(u => u.Password)
            .NotNull().WithMessage("É necessário informar a senha do usuário!")
            .NotEmpty().WithMessage("A senha do usuário não pode estar vazia!");
    }
}