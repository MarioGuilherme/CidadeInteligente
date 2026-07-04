using FluentValidation;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand>
{
    public UpdateAreaCommandValidator()
    {
        RuleFor(a => a.Description)
            .NotEmpty().WithMessage("É necessário informar a descrição da área!")
            .MaximumLength(45).WithMessage("A descrição da área não pode exceder 45 caracteres!");
    }
}
