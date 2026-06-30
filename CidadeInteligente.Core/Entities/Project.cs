namespace CidadeInteligente.Core.Entities;

public class Project(
    long areaId,
    long courseId,
    long createdByUserId,
    string title,
    string? description,
    DateOnly startedAt,
    DateOnly? finishedAt)
{
    public long ProjectId { get; private set; }
    public long CreatedByUserId { get; private set; } = createdByUserId;
    public User CreatedBy { get; private set; } = null!;
    public long AreaId { get; private set; } = areaId;
    public Area Area { get; private set; } = null!;
    public long CourseId { get; private set; } = courseId;
    public Course Course { get; private set; } = null!;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public DateOnly RegisteredAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly StartedAt { get; private set; } = startedAt;
    public DateOnly? FinishedAt { get; private set; } = finishedAt;
    public virtual ICollection<User> InvolvedUsers { get; set; } = [];
    public virtual ICollection<Media> Medias { get; private set; } = [];

    public void Update(long areaId, long courseId, string title, string? description, DateOnly startedAt, DateOnly? finishedAt)
    {
        AreaId = areaId;
        CourseId = courseId;
        Title = title;
        Description = description;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }
}
