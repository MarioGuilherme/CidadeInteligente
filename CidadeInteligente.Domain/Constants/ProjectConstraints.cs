namespace CidadeInteligente.Domain.Constants;

public static class ProjectConstraints
{
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 800;
    public static DateOnly StartedAtMaxDateOnly => DateOnly.FromDateTime(DateTime.UtcNow);
    public static DateOnly FinishedAtMaxDateOnly => DateOnly.FromDateTime(DateTime.UtcNow);
}
