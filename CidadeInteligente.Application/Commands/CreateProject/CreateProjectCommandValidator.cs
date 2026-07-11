using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(c => c.CurrentUserId).UserId(ValidationMessages.Project.CreatorRequired);
            RuleFor(c => c.Title).ProjectTitle();
            RuleFor(c => c.AreaId).AreaId(ValidationMessages.Project.AreaRequired);
            RuleFor(c => c.CourseId).CourseId(ValidationMessages.Project.CourseRequired);
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
