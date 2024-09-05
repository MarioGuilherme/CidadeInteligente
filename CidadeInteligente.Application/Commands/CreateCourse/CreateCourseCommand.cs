using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommand : IRequest<long> {
    public string Description { get; set; } = null!;
}