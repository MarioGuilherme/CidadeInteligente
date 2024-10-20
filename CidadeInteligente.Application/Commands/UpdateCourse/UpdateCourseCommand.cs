using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommand(string description) : IRequest<Unit> {
    public long CourseId { get; set; }
    public string Description { get; private set; } = description;
}