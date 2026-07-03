namespace CidadeInteligente.Application.Queries.GetProjects;

public record GetProjectsQueryResult(int Page, int TotalPages, int ItemsCount, IEnumerable<GetProjectsQueryResult.ProjectViewModel> Data)
{
    public record ProjectViewModel(int ProjectId, string Title, string? Description, IList<ProjectViewModel.MediaViewModel> Medias)
    {
        public string Description { get; private set; } = string.IsNullOrWhiteSpace(Description)
            ? string.Empty
            : Description.Length >= 125
                ? $"{Description![0..125]}..."
                : Description;

        public record MediaViewModel(int MediaId, string FileName, string MimeType)
        {
            public bool IsVideo { get; init; } = MimeType == "video/mp4";
            public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{FileName}";
        }
    }
}
