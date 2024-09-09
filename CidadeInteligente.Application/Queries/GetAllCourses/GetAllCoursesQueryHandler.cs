using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(ICourseRepository courseRepository, IMapper mapper) : IRequestHandler<GetAllCoursesQuery, List<CourseViewModel>> {
    private readonly ICourseRepository _courseRepository = courseRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<CourseViewModel>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
        List<Course> courses = await this._courseRepository.GetAllAsync();
        return this._mapper.Map<List<CourseViewModel>>(courses);
    }
}