using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(q => q.CourseId).CourseId();
    }
}
