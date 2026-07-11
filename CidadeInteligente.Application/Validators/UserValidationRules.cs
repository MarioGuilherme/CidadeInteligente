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
            .RequiredId(messageWhenEmpty ?? ValidationMessages.User.InvalidId);
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> UserName() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.User.NameRequired)
            .MaximumLength(UserConstraints.NameMaxLength).WithMessage(ValidationMessages.User.NameMaxLength);

        public IRuleBuilderOptions<T, string> UserEmail() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.User.EmailRequired)
            .EmailAddress().WithMessage(ValidationMessages.User.EmailInvalid)
            .MaximumLength(UserConstraints.EmailMaxLength).WithMessage(ValidationMessages.User.EmailMaxLength);

        public IRuleBuilderOptions<T, string> UserPassword() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.User.PasswordRequired)
            .MinimumLength(UserConstraints.RawPasswordMinLength).WithMessage(ValidationMessages.User.PasswordMinLength);

        public IRuleBuilderOptions<T, string> UserConfirmPassword(Expression<Func<T, string>> passwordSelector) => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.User.ConfirmPasswordRequired)
            .Equal(passwordSelector).WithMessage(ValidationMessages.User.PasswordsDoNotMatch);

        public IRuleBuilderOptions<T, string> UserToken() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.User.TokenRequired)
            .Length(UserConstraints.TokenRecoverPasswordMaxLength).WithMessage(ValidationMessages.User.TokenLength);
    }

    extension<T>(IRuleBuilder<T, Role> ruleBuilder)
    {
        public IRuleBuilderOptions<T, Role> UserRole() => ruleBuilder
            .IsInEnum().WithMessage(ValidationMessages.User.RoleRequired);
    }
}
