using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommand : IRequest<Unit> {
    public long CourseId { get; set; }
    public string Description { get; set; } = null!;
}