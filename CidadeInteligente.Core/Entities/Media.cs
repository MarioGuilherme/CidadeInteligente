namespace CidadeInteligente.Core.Entities;

public class Media
{
    public int MediaId { get; private set; }
    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public string FileName { get; private set; } = null!;

    public Media(string fileName) => FileName = fileName;

    public Media(string title, string? description, string fileName)
    {
        Title = title;
        Description = description;
        FileName = fileName;
    }

    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}
