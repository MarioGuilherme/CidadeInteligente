namespace CidadeInteligente.Domain.Constants;

public static class MediaConstraints
{
    public const int TitleMaxLength = 60;
    public const int FileNameMaxLength = 50;
    public const int DescriptionMaxLength = 300;
    public static int MimeTypeMaxLength => AllowedMimeTypes.Max(m => m.Length);

    public static readonly IReadOnlySet<string> AllowedMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpg",
        "image/jpeg",
        "video/mp4"
    };
}
