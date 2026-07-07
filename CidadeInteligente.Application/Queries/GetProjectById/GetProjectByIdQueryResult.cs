namespace CidadeInteligente.Application.Queries.GetProjectById;

public record GetProjectByIdQueryResult(int ProjectId,
    string Title,
    string Area,
    int AreaId,
    string Course,
    int CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<GetProjectByIdQueryResult.ProjectUserViewModel> InvolvedUsers,
    IEnumerable<GetProjectByIdQueryResult.MediaDetailsViewModel> Medias)
{
    public record ProjectUserViewModel(int UserId, string Name);

    public record MediaDetailsViewModel(int MediaId, string Title, string? Description, string Path, string MimeType)
    {
        public bool IsVideo { get; init; } = MimeType == "video/mp4";
    }
}
