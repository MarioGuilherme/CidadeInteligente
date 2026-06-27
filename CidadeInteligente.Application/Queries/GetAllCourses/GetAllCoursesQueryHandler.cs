using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCoursesQuery, List<CourseViewModel>> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<CourseViewModel>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
        List<Course> courses = await this._unitOfWork.Courses.GetAllAsync();
        return [.. courses.Select(c => new CourseViewModel(c.CourseId, c.Description))];
    }
}