using CidadeInteligente.Domain.Constants;
using FluentValidation;
using System.Linq.Expressions;

namespace CidadeInteligente.Application.Validators;

public static class ProjectValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> ProjectId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "O identificador do projeto é inválido");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> ProjectTitle() => ruleBuilder
            .NotEmpty().WithMessage("O título do projeto é obrigatório")
            .MaximumLength(ProjectConstraints.TitleMaxLength).WithMessage($"O título do projeto não pode exceder {ProjectConstraints.TitleMaxLength} caracteres");
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> ProjectDescription() => ruleBuilder
            .MaximumLength(ProjectConstraints.DescriptionMaxLength).WithMessage($"A descrição do projeto não pode exceder {ProjectConstraints.DescriptionMaxLength} caracteres");
    }

    extension<T>(IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly> ProjectStartedAt() => ruleBuilder
            .NotEmpty().WithMessage("A data de início do projeto é obrigatória")
            .LessThanOrEqualTo(_ => ProjectConstraints.StartedAtMaxDateOnly).WithMessage("A data de início do projeto não pode estar no futuro");
    }

    extension<T>(IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly?> ProjectFinishedAt(Expression<Func<T, DateOnly?>> startedAtSelector) => ruleBuilder
            .GreaterThanOrEqualTo(startedAtSelector).WithMessage("A data de término do projeto não pode ser anterior à data de início")
            .LessThanOrEqualTo(_ => ProjectConstraints.FinishedAtMaxDateOnly).WithMessage("A data de término do projeto não pode estar no futuro");
    }

    extension<T>(IRuleBuilder<T, IEnumerable<int>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<int>> ProjectInvolvedUsers() => ruleBuilder
            .NotEmpty().WithMessage("O projeto deve ter pelo menos um usuário envolvido")
            .Must(userIds => userIds.All(userId => userId > 0)).WithMessage("Informe um usuário válido");
    }

    extension<T, TMedia>(IRuleBuilder<T, IEnumerable<TMedia>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<TMedia>> ProjectMedias() => ruleBuilder
            .NotEmpty().WithMessage("O projeto deve ter pelo menos uma mídia anexada");
    }
}
