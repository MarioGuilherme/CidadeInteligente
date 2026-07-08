using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQueryValidator : AbstractValidator<GetAreaByIdQuery>
{
    public GetAreaByIdQueryValidator()
    {
        RuleFor(q => q.AreaId).AreaId();
    }
}
