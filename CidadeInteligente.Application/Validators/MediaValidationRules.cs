using CidadeInteligente.Domain.Constants;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public static class MediaValidationRules
{
    extension<T>(IRuleBuilder<T, int?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, int?> MediaId(string? messageWhenEmpty = default) => ruleBuilder
            .GreaterThan(0)
                .WithMessage(messageWhenEmpty ?? "O identificador da mídia é inválido");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaTitle() => ruleBuilder
            .NotEmpty().WithMessage("O título da mídia é obrigatório")
            .MaximumLength(MediaConstraints.TitleMaxLength)
                .WithMessage($"O título da mídia não pode exceder {MediaConstraints.TitleMaxLength} caracteres");
    }

    extension<T>(IRuleBuilderInitial<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> MediaMimeType() => ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O tipo (mime type) da mídia é obrigatório")
            .Must(mime => mime is not null && MediaConstraints.AllowedMimeTypes.Contains(mime))
                .WithMessage($"Tipo de mídia não suportado! Tipos aceitos: {string.Join(", ", MediaConstraints.AllowedMimeTypes)}");
    }

    extension<T>(IRuleBuilder<T, long> ruleBuilder)
    {
        public IRuleBuilderOptions<T, long> MediaFileSize() => ruleBuilder
            .LessThanOrEqualTo(MediaConstraints.FileMaxSizeInBytes)
                .WithMessage($"O arquivo da mídia não pode exceder {MediaConstraints.FileMaxSizeInMegaBytes} MB");
    }

    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> MediaDescription() => ruleBuilder
            .MaximumLength(MediaConstraints.DescriptionMaxLength)
                .WithMessage($"A descrição da mídia não pode exceder {MediaConstraints.DescriptionMaxLength} caracteres");
    }
}
