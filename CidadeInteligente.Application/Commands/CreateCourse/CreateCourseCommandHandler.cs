using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommandHandler(ICourseRepository courseRepository) : IRequestHandler<CreateCourseCommand, long> {
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<long> Handle(CreateCourseCommand request, CancellationToken cancellationToken) {
        Course course = new(request.Description);

        await this._courseRepository.AddAsync(course);

        return course.CourseId;
    }
}