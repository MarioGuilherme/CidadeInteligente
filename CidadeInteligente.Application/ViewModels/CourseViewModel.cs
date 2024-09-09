namespace CidadeInteligente.Application.ViewModels;

public class CourseViewModel(long courseId, string description) {
    public long CourseId { get; private set; } = courseId;
    public string Description { get; private set; } = description;
}