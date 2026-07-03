namespace CidadeInteligente.Domain.Entities;

public class Media(string title, string? description, string fileName, string mimeType)
{
    public int MediaId { get; private set; }
    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public string FileName { get; private set; } = fileName;
    public string MimeType { get; private set; } = mimeType;
    public string Extension { get; } = mimeType.Split('/').Last();

    public void Update(string title, string? description, string? mimeType = default)
    {
        Title = title;
        Description = description;
        if (!string.IsNullOrWhiteSpace(mimeType))
            MimeType = mimeType;
    }
}
