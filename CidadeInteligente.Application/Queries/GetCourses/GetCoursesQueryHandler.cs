using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourses;

public class GetCoursesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        Specification<Course, GetCoursesQueryResult.CourseViewModel> spec = SpecificationBuilder<Course>.Create()
            .WithProjection(a => new GetCoursesQueryResult.CourseViewModel(a.CourseId, a.Description))!;

        IEnumerable<GetCoursesQueryResult.CourseViewModel> course = await _unitOfWork.Courses.GetAllBySpecAsync(spec);
        return new(course);
    }
}
