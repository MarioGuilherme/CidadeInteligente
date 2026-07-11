using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class AreaValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> AreaId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "O identificador da área é inválido");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> AreaDescription() => ruleBuilder
            .NotEmpty().WithMessage("A descrição da área é obrigatória")
            .MaximumLength(AreaConstraints.DescriptionMaxLength).WithMessage($"A descrição da área não pode exceder {AreaConstraints.DescriptionMaxLength} caracteres");
    }
}
