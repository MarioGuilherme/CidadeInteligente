namespace CidadeInteligente.Core.Entities;

public class Course
{
    public long CourseId { get; private set; }
    public string Description { get; private set; }
    public virtual ICollection<Project> Projects { get; private set; } = [];

    public Course(string description) => Description = description;

    public Course(long courseId, string description)
    {
        CourseId = courseId;
        Description = description;
    }

    public void Update(string description)
    {
        Description = description;
    }
}