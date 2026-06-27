namespace CidadeInteligente.Application.Queries.GetCourses;

public record GetCoursesQueryResult(IEnumerable<GetCoursesQueryResult.CourseViewModel> Courses)
{
    public record CourseViewModel(long CourseId, string Description);
}
