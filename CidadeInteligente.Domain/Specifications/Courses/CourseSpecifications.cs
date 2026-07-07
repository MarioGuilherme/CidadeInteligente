using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Specifications.Courses;

public static class CourseSpecifications
{
    public static SpecificationBuilder<Course> GetById(int courseId)
    {
        return SpecificationBuilder<Course>.Create().Where(c => c.CourseId == courseId);
    }
}
