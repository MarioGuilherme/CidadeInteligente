namespace CidadeInteligente.Core.Entities;

public class Media(string title, string? description, string fileName, long size)
{
    public long MediaId { get; private set; }
    public long ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public string FileName { get; private set; } = fileName;
    public long Size { get; private set; } = size;
    public string Extension => System.IO.Path.GetExtension(FileName);
    public string Path => $"{Environment.GetEnvironmentVariable("AzureStorageBlobURL")}/{FileName}";

    public void Update(string title, string? description, long? size = default)
    {
        Title = title;
        Description = description;
        Size = size ?? Size;
    }
}