namespace CidadeInteligente.Application.ViewModels;

public class MediaViewModel(long mediaId, string fileName) {
    public long MediaId { get; private set; } = mediaId;
    public string FileName { get; private set; } = fileName;
    public string Extension => System.IO.Path.GetExtension(this.FileName);
    public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{this.FileName}";
}