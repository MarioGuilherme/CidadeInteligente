using CidadeInteligente.Application.Commands.UpdateUser;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand> {
    public UpdateUserCommandValidator() {
        this.RuleFor(u => u.UserId)
            .NotEmpty().WithMessage("É necessário informar o usuário!")
            .GreaterThan(0).WithMessage("Informe um identificador válido!");

        this.RuleFor(u => u.CourseId)
            .NotEmpty().WithMessage("É necessário informar o curso do usuário!")
            .GreaterThan(0).WithMessage("Informe um identificador válido!");

        this.RuleFor(u => u.Name)
            .NotNull().WithMessage("É necessário informar o nome do usuário!")
            .NotEmpty().WithMessage("O nome do usuário não pode estar em branco!")
            .MaximumLength(60).WithMessage("O nome do usuário não pode exceder 60 caracteres!");

        this.RuleFor(u => u.Email)
            .NotNull().WithMessage("É necessário informar o e-mail do usuário!")
            .NotEmpty().WithMessage("O e-mail do usuário não pode estar em branco!")
            .EmailAddress().WithMessage("Informe um e-mail válido!")
            .MaximumLength(60).WithMessage("O e-mail do usuário não pode exceder 60 caracteres!");

        this.RuleFor(u => u.Role)
            .NotEmpty().WithMessage("A permissão do usuário não pode estar em branco!")
            .IsInEnum().WithMessage("A permissão informada não é válida!");
    }
}