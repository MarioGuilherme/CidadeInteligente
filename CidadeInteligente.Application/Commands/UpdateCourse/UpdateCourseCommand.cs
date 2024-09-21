using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommand(long courseId, string description) : IRequest<Unit> {
    public long CourseId { get; private set; } = courseId;
    public string Description { get; private set; } = description;
}