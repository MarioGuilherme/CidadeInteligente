using FluentValidation;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryValidator : AbstractValidator<GetUserByTokenRecoverPasswordQuery>
{
    public GetUserByTokenRecoverPasswordQueryValidator() => RuleFor(g => g.Token).NotEmpty().Length(156);
}