using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class CourseValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> CourseId(string? messageWhenEmpty = default) => ruleBuilder
            .GreaterThan(0).WithMessage(messageWhenEmpty ?? "The course identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> CourseDescription() => ruleBuilder
            .NotEmpty().WithMessage("The course description is required")
            .MaximumLength(45).WithMessage("The course description cannot exceed 45 characters");
    }
}
