namespace CidadeInteligente.Core.Entities;

public class Area
{
    public long AreaId { get; private set; }
    public string Description { get; private set; }
    public virtual ICollection<Project> Projects { get; private set; } = [];

    public Area(long areaId, string description)
    {
        AreaId = areaId;
        Description = description;
    }

    public Area(string description)
    {
        Description = description;
    }

    public void Update(string description) => Description = description;
}
