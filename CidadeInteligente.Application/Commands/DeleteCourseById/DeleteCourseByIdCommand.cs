using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommand(long areaId) : IRequest<Unit?> {
    public long CourseId { get; private set; } = areaId;
}