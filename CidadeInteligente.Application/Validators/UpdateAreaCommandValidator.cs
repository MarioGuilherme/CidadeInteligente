using CidadeInteligente.Application.Commands.UpdateArea;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand> {
    public UpdateAreaCommandValidator() {
        this.RuleFor(a => a.AreaId)
            .GreaterThan(0).WithMessage("É necessário informar a área!");

        this.RuleFor(a => a.Description)
            .NotEmpty().WithMessage("É necessário informar a descrição da área!")
            .MaximumLength(45).WithMessage("A descrição da área não pode exceder 45 caracteres!");
    }
}