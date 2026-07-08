using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommandValidator : AbstractValidator<DeleteAreaByIdCommand>
{
    public DeleteAreaByIdCommandValidator() => RuleFor(c => c.AreaId).AreaId();
}
