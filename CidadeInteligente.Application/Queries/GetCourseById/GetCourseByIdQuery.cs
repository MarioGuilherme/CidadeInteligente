using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQuery(long courseId) : IRequest<Course?> {
    public long CourseId { get; private set; } = courseId;
}