using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetOrRemoveUserTokenRecoverPasswordCommandValidator : AbstractValidator<GetOrRemoveUserTokenRecoverPasswordCommand>
{
    public GetOrRemoveUserTokenRecoverPasswordCommandValidator() => RuleFor(c => c.Token).UserToken();
}
