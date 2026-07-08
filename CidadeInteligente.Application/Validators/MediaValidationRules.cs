using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class MediaValidationRules
{
    extension<T>(IRuleBuilder<T, int?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int?> MediaId(string? messageWhenEmpty = default) => ruleBuilder
            .GreaterThan(0).WithMessage(messageWhenEmpty ?? "The media identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaTitle() => ruleBuilder
            .NotEmpty().WithMessage("The media title is required")
            .MaximumLength(MediaConstraints.TitleMaxLength).WithMessage($"The media title cannot exceed {MediaConstraints.TitleMaxLength} characters");

        public IRuleBuilderOptions<T, string> MediaMimeType() => ruleBuilder
            .NotEmpty().WithMessage("The media mime type is required")
            .Must(mime => mime is not null && MediaConstraints.AllowedMimeTypes.Contains(mime)).WithMessage($"Unsupported media type! Accepted types: {string.Join(", ", MediaConstraints.AllowedMimeTypes)}");
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> MediaDescription() => ruleBuilder
            .MaximumLength(MediaConstraints.DescriptionMaxLength).WithMessage($"The media description cannot exceed {MediaConstraints.DescriptionMaxLength} characters");
    }

    extension<T>(IRuleBuilder<T, Func<Stream>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, Func<Stream>> MediaOpenStream() => ruleBuilder
            .NotNull().WithMessage("You need to attach the media file");
    }
}
