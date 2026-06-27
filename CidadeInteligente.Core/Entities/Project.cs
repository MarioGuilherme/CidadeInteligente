namespace CidadeInteligente.Core.Entities;

public class Project(long creatorUserId, long areaId, long courseId, string title, string? description, DateOnly registeredAt, DateOnly startedAt, DateOnly? finishedAt)
{
    public long CreatorUserId { get; private set; } = creatorUserId;
    public User CreatorUser { get; private set; }
    public long ProjectId { get; private set; }
    public long AreaId { get; private set; } = areaId;
    public Area Area { get; private set; }
    public long CourseId { get; private set; } = courseId;
    public Course Course { get; private set; }
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public DateOnly RegisteredAt { get; private set; } = registeredAt;
    public DateOnly StartedAt { get; private set; } = startedAt;
    public DateOnly? FinishedAt { get; private set; } = finishedAt;
    public List<User> InvolvedUsers { get; set; } = [];
    public List<Media> Medias { get; private set; } = [];

    public void Update(long areaId, long courseId, string title, string? description, DateOnly startedAt, DateOnly? finishedAt)
    {
        AreaId = areaId;
        CourseId = courseId;
        Title = title;
        Description = description;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }

    public override bool Equals(object? obj) => obj is Project project && ProjectId == project.ProjectId;

    public override int GetHashCode() => HashCode.Combine(ProjectId);
}