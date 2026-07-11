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
            .RequiredId(messageWhenEmpty ?? "O identificador do usuário é inválido");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> UserName() => ruleBuilder
            .NotEmpty().WithMessage("O nome do usuário é obrigatório")
            .MaximumLength(UserConstraints.NameMaxLength).WithMessage($"O nome do usuário não pode exceder {UserConstraints.NameMaxLength} caracteres");

        public IRuleBuilderOptions<T, string> UserEmail() => ruleBuilder
            .NotEmpty().WithMessage("O e-mail do usuário é obrigatório")
            .EmailAddress().WithMessage("O e-mail do usuário não é válido")
            .MaximumLength(UserConstraints.EmailMaxLength).WithMessage($"O e-mail do usuário não pode exceder {UserConstraints.EmailMaxLength} caracteres");

        public IRuleBuilderOptions<T, string> UserPassword() => ruleBuilder
            .NotEmpty().WithMessage("A nova senha é obrigatória")
            .MinimumLength(UserConstraints.RawPasswordMinLength).WithMessage($"A senha deve ter pelo menos {UserConstraints.RawPasswordMinLength} caracteres");

        public IRuleBuilderOptions<T, string> UserConfirmPassword(Expression<Func<T, string>> passwordSelector) => ruleBuilder
            .NotEmpty().WithMessage("A confirmação da senha é obrigatória")
            .Equal(passwordSelector).WithMessage("As senhas não conferem");

        public IRuleBuilderOptions<T, string> UserToken() => ruleBuilder
            .NotEmpty().WithMessage("O token de redefinição de senha é obrigatório")
            .Length(UserConstraints.TokenRecoverPasswordMaxLength).WithMessage($"O token de redefinição de senha deve ter {UserConstraints.TokenRecoverPasswordMaxLength} caracteres");
    }

    extension<T>(IRuleBuilder<T, Role> ruleBuilder)
    {
        public IRuleBuilderOptions<T, Role> UserRole() => ruleBuilder
            .IsInEnum().WithMessage("É necessário especificar uma permissão para o usuário!");
    }
}
