using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class GetUserByTokenRecoverPasswordQueryValidator : AbstractValidator<GetUserByTokenRecoverPasswordQuery>
{
    public GetUserByTokenRecoverPasswordQueryValidator() => RuleFor(g => g.Token).NotEmpty().Length(156);
}