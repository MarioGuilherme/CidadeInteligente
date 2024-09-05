using CidadeInteligente.Application.Commands.CreateMedia;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class CreateMediaCommandValidator : AbstractValidator<CreateMediaCommand> {
    public CreateMediaCommandValidator() {
        this.RuleFor(m => m.Title)
            .NotNull().WithMessage("É necessário informar o título da mídia!")
            .NotEmpty().WithMessage("O título da mídia não pode estar em branco!")
            .MaximumLength(60).WithMessage("O título da mídia não pode exceder 60 caracteres!");

        this.RuleFor(m => m.Description)
            .MaximumLength(300).WithMessage("A descrição da mídia não pode exceder 300 caracteres!");

        this.RuleFor(m => m.Extension)
            .NotNull().WithMessage("É necessário informar a extensão da mídia!")
            .NotEmpty().WithMessage("A extensão da mídia não pode estar em branco!");

        this.RuleFor(m => m.Base64)
            .NotNull().WithMessage("É necessário anexar o arquivo da mídia!")
            .NotEmpty().WithMessage("O arquivo da mídia não é um arquivo válido!");
    }
}