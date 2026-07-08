using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Enums;
using FluentValidation;
using System.Linq.Expressions;

namespace CidadeInteligente.Application.Validators;

public static class UserValidationRules
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int> UserId(string? messageWhenEmpty = default) => ruleBuilder
            .RequiredId(messageWhenEmpty ?? "The user identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> UserName() => ruleBuilder
            .NotEmpty().WithMessage("The user name is required")
            .MaximumLength(UserConstraints.NameMaxLength).WithMessage($"The user name cannot exceed {UserConstraints.NameMaxLength} characters");

        public IRuleBuilderOptions<T, string> UserEmail() => ruleBuilder
            .NotEmpty().WithMessage("The user email is required")
            .EmailAddress().WithMessage("The user email is not valid")
            .MaximumLength(UserConstraints.EmailMaxLength).WithMessage($"The user email cannot exceed {UserConstraints.EmailMaxLength} characters");

        public IRuleBuilderOptions<T, string> UserPassword() => ruleBuilder
            .NotEmpty().WithMessage("The new password is required")
            .MinimumLength(UserConstraints.RawPasswordMinLength).WithMessage($"The password must have at least {UserConstraints.RawPasswordMinLength} characters");

        public IRuleBuilderOptions<T, string> UserConfirmPassword(Expression<Func<T, string>> passwordSelector) => ruleBuilder
            .NotEmpty().WithMessage("The password confirmation is required")
            .Equal(passwordSelector).WithMessage("The passwords do not match");

        public IRuleBuilderOptions<T, string> UserToken() => ruleBuilder
            .NotEmpty().WithMessage("The token for password reset is required")
            .Length(UserConstraints.TokenRecoverPasswordMaxLength).WithMessage($"The password reset token must have {UserConstraints.TokenRecoverPasswordMaxLength} characters");
    }

    extension<T>(IRuleBuilder<T, Role> ruleBuilder)
    {
        public IRuleBuilderOptions<T, Role> UserRole() => ruleBuilder
            .IsInEnum().WithMessage("You must specify a permission for the user!");
    }
}
