using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class MediaValidationRules
{
    private static readonly HashSet<string> _allowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpg",
        "image/jpeg",
        "video/mp4"
    };

    extension<T>(IRuleBuilder<T, int?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int?> MediaId(string? messageWhenEmpty = default) => ruleBuilder
            .GreaterThan(0).WithMessage(messageWhenEmpty ?? "The media identifier is invalid");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaTitle() => ruleBuilder
            .NotEmpty().WithMessage("The media title is required")
            .MaximumLength(45).WithMessage("The media title cannot exceed 60 characters");

        public IRuleBuilderOptions<T, string> MediaMimeType() => ruleBuilder
            .NotEmpty().WithMessage("The media mime type is required")
            .Must(_allowedMimeTypes.Contains).WithMessage($"Unsupported media type! Accepted types: {string.Join(", ", _allowedMimeTypes)}");
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaDescription() => ruleBuilder
            .MaximumLength(300).WithMessage("The media description cannot exceed 300 characters");
    }

    extension<T>(IRuleBuilder<T, Func<Stream>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, Func<Stream>> MediaOpenStream() => ruleBuilder
            .NotNull().WithMessage("You need to attach the media file");
    }
}
