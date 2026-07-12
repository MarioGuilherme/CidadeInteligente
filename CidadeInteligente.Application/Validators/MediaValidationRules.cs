using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class MediaValidationRules
{
    extension<T>(IRuleBuilder<T, int?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int?> MediaId(string? messageWhenEmpty = default) => ruleBuilder
            .GreaterThan(0)
                .WithMessage(messageWhenEmpty ?? ValidationMessages.Media.InvalidId);
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaTitle() => ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Media.TitleRequired)
            .MaximumLength(MediaConstraints.TitleMaxLength)
                .WithMessage(ValidationMessages.Media.TitleMaxLength);
    }

    extension<T>(IRuleBuilderInitial<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaMimeType() => ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Media.MimeTypeRequired)
            .Must(mime => mime is not null && MediaConstraints.AllowedMimeTypes.Contains(mime))
                .WithMessage(ValidationMessages.Media.MimeTypeNotSupported);
    }

    extension<T>(IRuleBuilder<T, long> ruleBuilder)
    {
        public IRuleBuilderOptions<T, long> MediaFileSize() => ruleBuilder
            .LessThanOrEqualTo(MediaConstraints.FileMaxSizeInBytes)
                .WithMessage(ValidationMessages.Media.FileMaxSize);

        public IRuleBuilderOptions<T, long> MediaFileRequired() => ruleBuilder
            .GreaterThan(0).WithMessage(ValidationMessages.Media.FileRequired);
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> MediaDescription() => ruleBuilder
            .MaximumLength(MediaConstraints.DescriptionMaxLength)
                .WithMessage(ValidationMessages.Media.DescriptionMaxLength);
    }
}
