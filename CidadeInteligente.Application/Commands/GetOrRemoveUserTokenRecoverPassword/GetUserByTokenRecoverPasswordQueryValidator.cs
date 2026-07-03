using FluentValidation;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryValidator : AbstractValidator<GetUserByTokenRecoverPasswordQuery>
{
    public GetUserByTokenRecoverPasswordQueryValidator() => RuleFor(g => g.Token).NotEmpty().Length(156);
}