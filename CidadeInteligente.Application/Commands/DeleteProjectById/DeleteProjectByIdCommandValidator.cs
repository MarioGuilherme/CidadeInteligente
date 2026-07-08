using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommandValidator : AbstractValidator<DeleteProjectByIdCommand>
{
    public DeleteProjectByIdCommandValidator()
    {
        RuleFor(c => c.ProjectId).ProjectId();
        RuleFor(c => c.CurrentUserId).UserId();
    }
}
