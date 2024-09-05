using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(ICourseRepository courseRepository) : IRequestHandler<GetCourseByIdQuery, Course?> {
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<Course?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken) {
        Course? course = await this._courseRepository.GetByIdAsync(request.CourseId);
        return course;
    }
}