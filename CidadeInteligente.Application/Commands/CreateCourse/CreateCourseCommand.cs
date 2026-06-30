using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public record CreateCourseCommand(string Description) : IRequest<int?> { }
