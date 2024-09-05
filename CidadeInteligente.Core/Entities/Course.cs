namespace CidadeInteligente.Core.Entities;

public class Course(string description) {
    public long CourseId { get; private set; }
    public string Description { get; private set; } = description;
    public List<Project> Projects { get; private set; } = [];

    public void Update(string description) {
        this.Description = description;
    }
}