using CidadeInteligente.Application.Validators;
using FluentValidation;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(c => c.ProjectId).ProjectId();
        RuleFor(c => c.CurrentUserId).UserId(ValidationMessages.Project.CreatorRequired);
        RuleFor(c => c.Title).ProjectTitle();
        RuleFor(c => c.AreaId).AreaId(ValidationMessages.Project.AreaRequired);
        RuleFor(c => c.CourseId).CourseId(ValidationMessages.Project.CourseRequired);
        RuleFor(c => c.Description).ProjectDescription();
        RuleFor(c => c.StartedAt).ProjectStartedAt();
        RuleFor(c => c.FinishedAt).ProjectFinishedAt(c => c.StartedAt);
        RuleFor(c => c.InvolvedUsers).ProjectInvolvedUsers();
        RuleFor(c => c.Medias).ProjectMedias();
        RuleForEach(c => c.Medias).SetValidator(new UpdateMediaCommandValidator());
    }

    public class UpdateMediaCommandValidator : AbstractValidator<UpdateProjectCommand.UpdateMediaCommand>
    {
        public UpdateMediaCommandValidator()
        {
            RuleFor(c => c.MediaId).MediaId();
            RuleFor(c => c.Title).MediaTitle();
            RuleFor(c => c.Description).MediaDescription();
            RuleFor(c => c.MimeType).MediaMimeType().When(c => c.FileSize > 0);
            RuleFor(c => c.FileSize).MediaFileRequired().When(c => c.MediaId is null);
            RuleFor(c => c.FileSize).MediaFileSize();
        }
    }
}
