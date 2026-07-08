using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandValidator : AbstractValidator<DeleteCourseByIdCommand>
{
    public DeleteCourseByIdCommandValidator() => RuleFor(c => c.CourseId).CourseId();
}
