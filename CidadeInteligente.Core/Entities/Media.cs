namespace CidadeInteligente.Core.Entities;

public class Media(string title, string? description, string fileName)
{
    public int MediaId { get; private set; }
    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public string FileName { get; private set; } = fileName;

    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}
