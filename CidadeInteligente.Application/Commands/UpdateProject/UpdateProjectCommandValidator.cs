using FluentValidation;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("É necessário informar o título do projeto!")
            .MaximumLength(100).WithMessage("O título do projeto não pode exceder 100 caracteres!");

        RuleFor(p => p.AreaId)
            .GreaterThan(0).WithMessage("É necessário informar a área do projeto!");

        RuleFor(p => p.CourseId)
            .GreaterThan(0).WithMessage("É necessário informar o curso do projeto!");

        RuleFor(p => p.Description)
            .MaximumLength(800).WithMessage("A descrição do projeto não pode exceder 800 caracteres!");

        RuleFor(p => p.StartedAt)
            .NotEmpty().WithMessage("É necessário informar a data de ínicio do projeto!")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("A data de início não pode ser uma data futura!");

        RuleFor(p => p.FinishedAt)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("A data de início não pode ser uma data futura!");

        RuleFor(p => p.InvolvedUsers)
            .NotEmpty().WithMessage("O projeto precisa ter pelo menos um usuário envolvido!")
            .Must(userIds => userIds.All(userId => userId > 0))
            .WithMessage("Informe um usuário válido!");

        RuleFor(p => p.Medias)
            .NotEmpty().WithMessage("O projeto precisa ter pelo menos uma mídia em anexo!");

        RuleForEach(p => p.Medias)
            .SetValidator(new UpdateMediaCommandValidator());
    }

    public class UpdateMediaCommandValidator : AbstractValidator<UpdateProjectCommand.UpdateMediaCommand>
    {
        public UpdateMediaCommandValidator()
        {
            RuleFor(m => m.MediaId)
                .GreaterThan(0).WithMessage("É necessário informar a mídia!");

            RuleFor(m => m.Title)
                .NotEmpty().WithMessage("É necessário informar o título da mídia!")
                .MaximumLength(60).WithMessage("O título da mídia não pode exceder 60 caracteres!");

            RuleFor(m => m.Description)
                .MaximumLength(300).WithMessage("A descrição da mídia não pode exceder 300 caracteres!");

            RuleFor(m => m.Extension)
                .NotEmpty().WithMessage("É necessário informar a extensão da mídia!");

            //When(m => m.OpenStream is null, () =>
            //{
            //    RuleFor(m => m.Base64)
            //        .NotEmpty().WithMessage("É necessário anexar o arquivo da mídia!");
            //}).Otherwise(() => RuleFor(m => m.Path).NotEmpty().WithMessage("É necessário informar a URL da mídias!"));
        }
    }
}