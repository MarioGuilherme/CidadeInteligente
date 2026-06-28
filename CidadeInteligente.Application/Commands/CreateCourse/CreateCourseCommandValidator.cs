using FluentValidation;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("É necessário informar a descrição do curso!")
            .MaximumLength(45).WithMessage("A descrição do curso não pode exceder 45 caracteres!");
    }
}