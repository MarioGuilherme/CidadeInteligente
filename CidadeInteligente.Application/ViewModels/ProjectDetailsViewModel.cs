namespace CidadeInteligente.Application.ViewModels;

public class ProjectDetailsViewModel(long projectId, string title, string area, long areaId, string course, long courseId, string description, DateOnly startedAt, DateOnly? finishedAt, List<ProjectUserViewModel> involvedUsers, List<MediaDetailsViewModel> medias)
{
    public long ProjectId { get; private set; } = projectId;
    public string Title { get; private set; } = title;
    public string Area { get; private set; } = area;
    public long AreaId { get; private set; } = areaId;
    public string Course { get; private set; } = course;
    public long CourseId { get; private set; } = courseId;
    public string Description { get; private set; } = description;
    public DateOnly StartedAt { get; private set; } = startedAt;
    public DateOnly? FinishedAt { get; private set; } = finishedAt;
    public List<ProjectUserViewModel> InvolvedUsers { get; private set; } = involvedUsers;
    public List<MediaDetailsViewModel> Medias { get; private set; } = medias;
}