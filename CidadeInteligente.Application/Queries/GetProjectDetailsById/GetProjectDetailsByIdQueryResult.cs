namespace CidadeInteligente.Application.Queries.GetProjectDetailsById;

public record GetProjectDetailsByIdQueryResult(long ProjectId,
    string Title,
    string Area,
    long AreaId,
    string Course,
    long CourseId,
    string Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<GetProjectDetailsByIdQueryResult.ProjectUserViewModel> InvolvedUsers,
    IEnumerable<GetProjectDetailsByIdQueryResult.MediaDetailsViewModel> Medias)
{
    public record ProjectUserViewModel(long UserId, string Name)
    {
        public string MinorName => Name.Length > 58 ? Name[0..58] : Name;
    }

    public record MediaDetailsViewModel(long MediaId, string Title, string? Description, string FileName, long Size)
    {
        public string Extension => System.IO.Path.GetExtension(FileName);
        public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{FileName}";
    }
}
