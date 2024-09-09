namespace CidadeInteligente.Application.ViewModels;

public class ProjectViewModel(long projectId, string title, string? description, List<MediaViewModel> medias) {
    public long ProjectId { get; private set; } = projectId;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description is null
        ? "Sem descrição"
        : description.Length >= 125
            ? $"{description![0..125]}..."
            : description;
    public List<MediaViewModel> Medias { get; private set; } = medias;
}