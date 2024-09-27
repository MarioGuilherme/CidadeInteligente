using CidadeInteligente.Application.Commands.CreateUser;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
    public CreateUserCommandValidator() {
        this.RuleFor(u => u.CourseId).GreaterThan(0).WithMessage("É necessário informar um curso válido!");

        this.RuleFor(u => u.Name)
            .NotEmpty().WithMessage("É necessário informar o nome do usuário!")
            .MaximumLength(60).WithMessage("O nome do usuário não pode exceder 60 caracteres!");

        this.RuleFor(u => u.Email)
            .NotEmpty().WithMessage("É necessário informar o e-mail do usuário!")
            .EmailAddress().WithMessage("Informe um e-mail válido!")
            .MaximumLength(60).WithMessage("O e-mail do usuário não pode exceder 60 caracteres!");

        this.RuleFor(u => u.Password)
            .NotEmpty().WithMessage("É necessário informar uma senha para o usuário!");

        this.RuleFor(u => u.Role)
            .IsInEnum().WithMessage("É necessário informar uma permissão para o usuário!");
    }
}