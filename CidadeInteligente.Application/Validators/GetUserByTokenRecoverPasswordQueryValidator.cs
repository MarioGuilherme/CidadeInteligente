using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class GetUserByTokenRecoverPasswordQueryValidator : AbstractValidator<GetUserByTokenRecoverPasswordQuery> {
    public GetUserByTokenRecoverPasswordQueryValidator() => this.RuleFor(g => g.Token).NotEmpty().Length(156);
}