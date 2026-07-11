using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class AreaValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> AreaId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? ValidationMessages.Area.InvalidId);
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> AreaDescription() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Area.DescriptionRequired)
            .MaximumLength(AreaConstraints.DescriptionMaxLength).WithMessage(ValidationMessages.Area.DescriptionMaxLength);
    }
}
