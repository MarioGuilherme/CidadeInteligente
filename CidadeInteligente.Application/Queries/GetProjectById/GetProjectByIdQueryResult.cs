namespace CidadeInteligente.Application.Queries.GetProjectById;

public record GetProjectByIdQueryResult(int ProjectId,
    string Title,
    string Area,
    int AreaId,
    string Course,
    int CourseId,
    string Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<GetProjectByIdQueryResult.ProjectUserViewModel> InvolvedUsers,
    IEnumerable<GetProjectByIdQueryResult.MediaDetailsViewModel> Medias)
{
    public record ProjectUserViewModel(int UserId, string Name)
    {
        public string MinorName => Name.Length > 58 ? Name[0..58] : Name;
    }

    public record MediaDetailsViewModel(int MediaId, string Title, string? Description, string FileName)
    {
        public string Extension => System.IO.Path.GetExtension(FileName);
        public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{FileName}";
    }
}
