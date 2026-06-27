using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourses;

public class GetCoursesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        List<Course> courses = await _unitOfWork.Courses.GetAllAsync();
        return new(courses.Select(c => new GetCoursesQueryResult.CourseViewModel(c.CourseId, c.Description)));
    }
}
