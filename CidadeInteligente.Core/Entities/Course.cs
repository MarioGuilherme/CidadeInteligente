namespace CidadeInteligente.Core.Entities;

public class Course {
    public long CourseId { get; private set; }
    public string Description { get; private set; }
    public List<Project> Projects { get; private set; } = [];

    public Course(string description) => this.Description = description;

    public Course(long courseId, string description) {
        this.CourseId = courseId;
        this.Description = description;
    }

    public void Update(string description) {
        this.Description = description;
    }
}