using CidadeInteligente.Domain.Constants;
using FluentValidation;
using System.Linq.Expressions;

namespace CidadeInteligente.Application.Validators;

public static class ProjectValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> ProjectId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? ValidationMessages.Project.InvalidId);
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> ProjectTitle() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Project.TitleRequired)
            .MaximumLength(ProjectConstraints.TitleMaxLength).WithMessage(ValidationMessages.Project.TitleMaxLength);
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> ProjectDescription() => ruleBuilder
            .MaximumLength(ProjectConstraints.DescriptionMaxLength).WithMessage(ValidationMessages.Project.DescriptionMaxLength);
    }

    extension<T>(IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly> ProjectStartedAt() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Project.StartedAtRequired)
            .LessThanOrEqualTo(_ => ProjectConstraints.StartedAtMaxDateOnly).WithMessage(ValidationMessages.Project.StartedAtInFuture);
    }

    extension<T>(IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly?> ProjectFinishedAt(Expression<Func<T, DateOnly?>> startedAtSelector) => ruleBuilder
            .GreaterThanOrEqualTo(startedAtSelector).WithMessage(ValidationMessages.Project.FinishedAtBeforeStartedAt)
            .LessThanOrEqualTo(_ => ProjectConstraints.FinishedAtMaxDateOnly).WithMessage(ValidationMessages.Project.FinishedAtInFuture);
    }

    extension<T>(IRuleBuilder<T, IEnumerable<int>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<int>> ProjectInvolvedUsers() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Project.InvolvedUsersRequired)
            .Must(userIds => userIds.All(userId => userId > 0)).WithMessage(ValidationMessages.Project.InvalidInvolvedUser);
    }

    extension<T, TMedia>(IRuleBuilder<T, IEnumerable<TMedia>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<TMedia>> ProjectMedias() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Project.MediasRequired);
    }
}
