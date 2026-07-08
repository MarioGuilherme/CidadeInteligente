using FluentValidation;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordCommandValidator : AbstractValidator<GetOrRemoveUserTokenRecoverPasswordCommandResult>
{
    public GetUserByTokenRecoverPasswordCommandValidator() => RuleFor(g => g.Token).NotEmpty().Length(156);
}
