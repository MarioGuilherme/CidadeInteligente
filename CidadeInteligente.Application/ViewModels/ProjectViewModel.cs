using CidadeInteligente.Core.Entities;

namespace CidadeInteligente.Application.ViewModels;

public class ProjectViewModel(long creatorUserId, long projectId, long areaId, long courseId, string title, string? description, DateOnly registeredAt, DateOnly startedAt, DateOnly? finishedAt, List<Media> medias) {
    public long CreatorUserId { get; private set; } = creatorUserId;
    public long ProjectId { get; private set; } = projectId;
    public long AreaId { get; private set; } = areaId;
    public long CourseId { get; private set; } = courseId;
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public DateOnly RegisteredAt { get; private set; } = registeredAt;
    public DateOnly StartAt { get; private set; } = startedAt;
    public DateOnly? FinishedAt { get; private set; } = finishedAt;
    public List<Media> Medias { get; set; } = medias;
}