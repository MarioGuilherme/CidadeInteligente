namespace CidadeInteligente.Core.Entities;

public class Project
{
    public int ProjectId { get; private set; }
    public int CreatedByUserId { get; private set; }
    public User CreatedBy { get; private set; } = null!;
    public int AreaId { get; private set; }
    public Area Area { get; private set; } = null!;
    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; } = null!;
    public DateOnly RegisteredAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly StartedAt { get; private set; }
    public DateOnly? FinishedAt { get; private set; }
    public virtual ICollection<User> InvolvedUsers { get; set; } = [];
    public virtual ICollection<Media> Medias { get; private set; } = [];

    public Project(int projectId, string title, string? description, Media firstMedia)
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        Medias = [firstMedia];
    }

    public Project(int projectId) => ProjectId = projectId;

    public Project(int projectId, string title, string? description, ICollection<Media> medias)
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        Medias = medias;
    }

    public Project(
        int areaId,
        int courseId,
        int createdByUserId,
        string title,
        string? description,
        DateOnly startedAt,
        DateOnly? finishedAt)
    {
        CreatedByUserId = createdByUserId;
        AreaId = areaId;
        CourseId = courseId;
        Title = title;
        Description = description;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }

    public Project(int projectId,
        Area area,
        Course course,
        string title,
        string? description,
        DateOnly startedAt,
        DateOnly? finishedAt,
        ICollection<User> involvedUsers,
        ICollection<Media> medias)
    {
        ProjectId = projectId;
        Area = area;
        Course = course;
        Title = title;
        Description = description;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        InvolvedUsers = involvedUsers;
        Medias = medias;
    }

    public void Update(int areaId, int courseId, string title, string? description, DateOnly startedAt, DateOnly? finishedAt)
    {
        AreaId = areaId;
        CourseId = courseId;
        Title = title;
        Description = description;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }
}
