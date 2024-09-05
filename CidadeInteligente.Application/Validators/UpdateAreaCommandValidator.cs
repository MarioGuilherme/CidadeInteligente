using CidadeInteligente.Application.Commands.UpdateArea;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand> {
    public UpdateAreaCommandValidator() {
        this.RuleFor(a => a.Description)
            .NotNull().WithMessage("É necessário informar a descrição da área!")
            .NotEmpty().WithMessage("A descrição da área não pode estar em branco!")
            .MaximumLength(45).WithMessage("A descrição da área não pode exceder 45 caracteres!");
    }
}