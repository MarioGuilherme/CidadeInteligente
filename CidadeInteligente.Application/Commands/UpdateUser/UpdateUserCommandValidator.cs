using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.UserId).UserId();
        RuleFor(c => c.CourseId).CourseId(ValidationMessages.User.CourseRequired);
        RuleFor(c => c.Name).UserName();
        RuleFor(c => c.Email).UserEmail();
        RuleFor(c => c.Role).UserRole();
    }
}
