using CidadeInteligente.Core.Models;

namespace CidadeInteligente.Application.Queries.GetProjects;

public record GetProjectsQueryResult(int Page, int TotalPages, int ItemsCount, IEnumerable<GetProjectsQueryResult.ProjectViewModel> Data)
    : PaginationResult<GetProjectsQueryResult.ProjectViewModel>(Page, TotalPages, ItemsCount, Data)
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
