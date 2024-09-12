namespace CidadeInteligente.Core.Entities;

public class Media(string title, string? description, string fileName, long size) {
    public long MediaId { get; private set; }
    public long ProjectId { get; private set; }
    public Project Project { get; private set; }
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public string FileName { get; private set; } = fileName;
    public long Size { get; private set; } = size;
    public string Extension => System.IO.Path.GetExtension(this.FileName);
    public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{this.FileName}";

    public void Update(string title, string? description) {
        this.Title = title;
        this.Description = description;
    }

    public void Update(string title, string? description, long size) {
        this.Title = title;
        this.Description = description;
        this.Size = size;
    }
}