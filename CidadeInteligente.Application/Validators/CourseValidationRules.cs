using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class CourseValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> CourseId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "O identificador do curso é inválido");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> CourseDescription() => ruleBuilder
            .NotEmpty().WithMessage("A descrição do curso é obrigatória")
            .MaximumLength(CourseConstraints.DescriptionMaxLength).WithMessage($"A descrição do curso não pode exceder {CourseConstraints.DescriptionMaxLength} caracteres");
    }
}
