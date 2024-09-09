namespace CidadeInteligente.Application.ViewModels;

public class ProjectDetailsViewModel {
    public long ProjectId { get; private set; }
    public string Title { get; private set; } = null!;
    public string Area { get; private set; } = null!;
    public long AreaId { get; private set; }
    public string Course { get; private set; } = null!;
    public long CourseId { get; private set; }
    public string Description { get; private set; } = null!;
    public DateOnly StartedAt { get; private set; }
    public DateOnly? FinishedAt { get; private set; }
    public List<ProjectUserViewModel> InvolvedUsers { get; private set; } = [];
    public List<MediaDetailsViewModel> Medias { get; private set; } = [];
}