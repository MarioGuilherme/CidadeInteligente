using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public record UpdateCourseCommand(long CourseId, string Description) : IRequest<Unit?> { }
