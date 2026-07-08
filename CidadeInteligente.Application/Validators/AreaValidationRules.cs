using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class AreaValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> AreaId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "The area identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> AreaDescription() => ruleBuilder
            .NotEmpty().WithMessage("The area description is required")
            .MaximumLength(45).WithMessage("The area description cannot exceed 45 characters");
    }
}
