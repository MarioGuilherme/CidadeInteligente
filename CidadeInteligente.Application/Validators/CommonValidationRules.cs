using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class CommonValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> RequiredId(string message) => ruleBuilder
            .GreaterThan(0).WithMessage(message);

        public IRuleBuilderOptions<T, int> RequiredPage() => ruleBuilder
            .GreaterThan(0).WithMessage("The page is invalid");
    }
}
