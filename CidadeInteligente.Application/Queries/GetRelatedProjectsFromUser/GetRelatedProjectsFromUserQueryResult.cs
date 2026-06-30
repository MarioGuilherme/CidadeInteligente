using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public record GetRelatedProjectsFromUserQueryResult(int Page, int TotalPages, int ItemsCount, IEnumerable<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> Data)
    : PaginationResult<GetRelatedProjectsFromUserQueryResult.ProjectViewModel>(Page, TotalPages, ItemsCount, Data)
{
    public record ProjectViewModel(long ProjectId, string Title, string? Description, IList<ProjectViewModel.MediaViewModel> Medias)
    {
        public string Description { get; private set; } = string.IsNullOrWhiteSpace(Description)
            ? string.Empty
            : Description.Length >= 125
                ? $"{Description![0..125]}..."
                : Description;

        public record MediaViewModel(long MediaId, string FileName)
        {
            public string Extension => System.IO.Path.GetExtension(FileName);
            public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{FileName}";
        }
    }
}
