namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public record GetRelatedProjectsFromUserQueryResult(int Page, int TotalPages, int ItemsCount, IReadOnlyList<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> Data)
{
    public record ProjectViewModel(int ProjectId, string Title, string? Description, IEnumerable<ProjectViewModel.MediaViewModel> Medias)
    {
        public record MediaViewModel(int MediaId, string Path, string MimeType)
        {
            public bool IsVideo { get; init; } = MimeType == "video/mp4";
        }
    }
}
