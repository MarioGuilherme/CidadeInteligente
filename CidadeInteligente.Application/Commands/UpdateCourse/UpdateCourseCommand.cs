using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public record UpdateCourseCommand(int CourseId, string Description) : IRequest<Unit?> { }
