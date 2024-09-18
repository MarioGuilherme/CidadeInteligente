using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllCoursesQuery, List<CourseViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<CourseViewModel>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
        List<Course> courses = await this._unitOfWork.Courses.GetAllAsync();
        return this._mapper.Map<List<CourseViewModel>>(courses);
    }
}