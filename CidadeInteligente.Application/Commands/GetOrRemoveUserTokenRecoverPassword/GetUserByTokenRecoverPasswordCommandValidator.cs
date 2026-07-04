using FluentValidation;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordCommandValidator : AbstractValidator<GetUserByTokenRecoverPasswordCommand>
{
    public GetUserByTokenRecoverPasswordCommandValidator() => RuleFor(g => g.Token).NotEmpty().Length(156);
}
