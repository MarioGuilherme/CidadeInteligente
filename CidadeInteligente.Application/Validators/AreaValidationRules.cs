using CidadeInteligente.Domain.Constants;
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
            .MaximumLength(AreaConstraints.DescriptionMaxLength).WithMessage($"The area description cannot exceed {AreaConstraints.DescriptionMaxLength} characters");
    }
}
