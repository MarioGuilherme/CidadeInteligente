using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourses;

public class GetCoursesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        Specification<Course, GetCoursesQueryResult.CourseViewModel> getCoursesSpec = SpecificationBuilder<Course>.Create()
            .WithProjection<GetCoursesQueryResult.CourseViewModel>(a => new(a.CourseId, a.Description))!;

        IEnumerable<GetCoursesQueryResult.CourseViewModel> course = await _unitOfWork.Courses.GetAllBySpecAsync(getCoursesSpec, cancellationToken);
        return new(course);
    }
}
