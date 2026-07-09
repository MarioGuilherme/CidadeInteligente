using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(c => c.CurrentUserId).UserId("It is necessary to specify the project creator");
            RuleFor(c => c.Title).ProjectTitle();
            RuleFor(c => c.AreaId).AreaId("It is necessary to specify the project area");
            RuleFor(c => c.CourseId).CourseId("It is necessary to specify the project course");
            RuleFor(c => c.Description).ProjectDescription();
            RuleFor(c => c.StartedAt).ProjectStartedAt();
            RuleFor(c => c.FinishedAt).ProjectFinishedAt(c => c.StartedAt);
            RuleFor(c => c.InvolvedUsers).ProjectInvolvedUsers();
            RuleFor(c => c.Medias).ProjectMedias();
            RuleForEach(c => c.Medias).SetValidator(new CreateMediaCommandValidator());
        }

        public class CreateMediaCommandValidator : AbstractValidator<CreateProjectCommand.CreateMediaCommand>
        {
            public CreateMediaCommandValidator()
            {
                RuleFor(c => c.Title).MediaTitle();
                RuleFor(c => c.Description).MediaDescription();
                RuleFor(c => c.MimeType).MediaMimeType();
                RuleFor(c => c.FileSize).MediaFileSize();
            }
        }
    }
}
