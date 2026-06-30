using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public record DeleteCourseByIdCommand(int CourseId) : IRequest<Unit?> { }
