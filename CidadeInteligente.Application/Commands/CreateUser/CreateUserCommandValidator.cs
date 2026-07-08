using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.CourseId).CourseId("It is necessary to specify the user course");
        RuleFor(c => c.Name).UserName();
        RuleFor(c => c.Email).UserEmail();
        RuleFor(c => c.Password).UserPassword();
        RuleFor(c => c.ConfirmPassword).UserConfirmPassword(c => c.Password);
        RuleFor(c => c.Role).UserRole();
    }
}
