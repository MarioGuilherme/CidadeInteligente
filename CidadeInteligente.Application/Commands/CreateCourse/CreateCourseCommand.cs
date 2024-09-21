using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommand(string description) : IRequest<long> {
    public string Description { get; private set; } = description;
}