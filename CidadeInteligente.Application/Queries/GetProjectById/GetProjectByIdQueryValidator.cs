using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetProjectById;

public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(q => q.ProjectId).ProjectId();
    }
}
