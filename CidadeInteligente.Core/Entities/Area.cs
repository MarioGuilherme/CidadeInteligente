namespace CidadeInteligente.Core.Entities;

public class Area
{
    public int AreaId { get; private set; }
    public string Description { get; private set; } = null!;
    public virtual ICollection<Project> Projects { get; private set; } = [];

    public Area(int areaId, string description)
    {
        AreaId = areaId;
        Description = description;
    }

    public Area(IEnumerable<Project> projects) => Projects = [.. projects];

    public Area(string description)
    {
        Description = description;
    }

    public void Update(string description) => Description = description;
}
