using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Domain.Entities;
using FluentValidation;
using System.Linq.Expressions;

namespace CidadeInteligente.Application.Validators;

public static class ProjectValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> ProjectId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "The project identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> ProjectTitle() => ruleBuilder
            .NotEmpty().WithMessage("The project title is required")
            .MaximumLength(100).WithMessage("The project title cannot exceed 100 characters");
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> ProjectDescription() => ruleBuilder
            .MaximumLength(800).WithMessage("The project description cannot exceed 800 characters");
    }

    extension<T>(IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly> ProjectStartedAt() => ruleBuilder
            .NotEmpty().WithMessage("The project start date is required")
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Now)).WithMessage("The project start date cannot be in the future");
    }

    extension<T>(IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly?> ProjectFinishedAt(Expression<Func<T, DateOnly?>> startedAtSelector) => ruleBuilder
            .GreaterThanOrEqualTo(startedAtSelector).WithMessage("The project finish date cannot be before the start date")
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Now)).WithMessage("The project finish date cannot be in the future");
    }

    extension<T>(IRuleBuilder<T, IEnumerable<int>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<int>> ProjectInvolvedUsers() => ruleBuilder
            .NotEmpty().WithMessage("The project must have at least one involved user")
            .Must(userIds => userIds.All(userId => userId > 0)).WithMessage("Please specify a valid user");
    }

    extension<T, TMedia>(IRuleBuilder<T, IEnumerable<TMedia>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, IEnumerable<TMedia>> ProjectMedias() => ruleBuilder
            .NotEmpty().WithMessage("The project must have at least one media attached");
    }
}
