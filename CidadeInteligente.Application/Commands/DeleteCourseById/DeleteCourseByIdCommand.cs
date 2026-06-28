using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public record DeleteCourseByIdCommand(long CourseId) : IRequest<Unit?> { }
