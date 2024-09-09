namespace CidadeInteligente.Application.ViewModels;

public class MediaDetailsViewModel(long mediaId, string title, string? description, string fileName, long size) {
    public long MediaId { get; private set; } = mediaId;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public string FileName { get; private set; } = fileName;
    public long Size { get; private set; } = size;
    public string Extension => System.IO.Path.GetExtension(this.FileName);
    public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBaseURL")}{Environment.GetEnvironmentVariable("AzureStorageContainerName")}/{this.FileName}";
}