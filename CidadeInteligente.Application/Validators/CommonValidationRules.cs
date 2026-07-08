using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class CommonValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> Page() => ruleBuilder
            .GreaterThan(0).WithMessage("The page number is invalid");
    }
}
