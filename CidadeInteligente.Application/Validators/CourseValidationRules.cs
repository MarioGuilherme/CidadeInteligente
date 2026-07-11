using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class CourseValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> CourseId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? ValidationMessages.Course.InvalidId);
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> CourseDescription() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Course.DescriptionRequired)
            .MaximumLength(CourseConstraints.DescriptionMaxLength).WithMessage(ValidationMessages.Course.DescriptionMaxLength);
    }
}
