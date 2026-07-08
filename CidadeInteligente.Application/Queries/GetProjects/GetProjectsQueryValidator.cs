using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetProjects;

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator() => RuleFor(q => q.Page).RequiredPage();
}
