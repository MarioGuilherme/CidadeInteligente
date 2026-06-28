using FluentValidation;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommandValidator : AbstractValidator<CreateAreaCommand>
{
    public CreateAreaCommandValidator()
    {
        RuleFor(a => a.Description)
            .NotEmpty().WithMessage("É necessário informar a descrição da área!")
            .MaximumLength(45).WithMessage("A descrição da área não pode exceder 45 caracteres!");
    }
}
