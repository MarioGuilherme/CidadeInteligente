using CidadeInteligente.Application.Commands.CreateProject;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

internal class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand> {
    public CreateProjectCommandValidator() {
        this.RuleFor(p => p.Title)
            .NotNull().WithMessage("É necessário informar o título do projeto!")
            .NotEmpty().WithMessage("O título do projeto não pode estar em branco!")
            .MaximumLength(100).WithMessage("O título do projeto não pode exceder 100 caracteres!");

        this.RuleFor(p => p.AreaId)
            .NotEmpty().WithMessage("É necessário informar a área do projeto!")
            .GreaterThan(0).WithMessage("Informe um identificador válido!");

        this.RuleFor(p => p.CourseId)
            .NotEmpty().WithMessage("É necessário informar o curso do projeto!")
            .GreaterThan(0).WithMessage("Informe um identificador válido!");

        this.RuleFor(p => p.Description)
            .MaximumLength(800).WithMessage("A descrição do projeto não pode exceder 800 caracteres!");

        this.RuleFor(p => p.StartedAt)
            .NotNull().WithMessage("É necessário informar a data de ínicio do projeto!")
            .NotEmpty().WithMessage("A data de ínicio do projeto não pode estar em branco!")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("A data de início não pode ser uma data futura!");

        this.RuleFor(p => p.FinishedAt)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("A data de início não pode ser uma data futura!");

        this.RuleFor(p => p.InvolvedUsers)
            .NotEmpty().WithMessage("O projeto precisa ter pelo menos um usuário envolvido!")
            .Must(userIds => userIds.All(userId => userId > 0))
            .WithMessage("Informe um identificador válido!");

        this.RuleFor(p => p.Medias)
            .NotEmpty().WithMessage("O projeto precisa ter pelo menos uma mídia em anexo!");

        this.RuleForEach(p => p.Medias)
            .SetValidator(new CreateMediaCommandValidator());
    }
}