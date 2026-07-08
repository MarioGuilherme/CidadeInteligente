using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryValidator : AbstractValidator<GetRelatedProjectsFromUserQuery>
{
    public GetRelatedProjectsFromUserQueryValidator()
    {
        RuleFor(q => q.UserId).UserId();
        RuleFor(q => q.Page).Page();
    }
}
