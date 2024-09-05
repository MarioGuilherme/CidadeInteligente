using CidadeInteligente.Application.Queries.GetAllCourse;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(ICourseRepository courseRepository) : IRequestHandler<GetAllCoursesQuery, List<Course>> {
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<List<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
        List<Course> courses = await this._courseRepository.GetAllAsync();
        return courses;
    }
}