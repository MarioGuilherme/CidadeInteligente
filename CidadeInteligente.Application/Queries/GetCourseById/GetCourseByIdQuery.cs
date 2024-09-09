using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQuery(long courseId) : IRequest<CourseViewModel?> {
    public long CourseId { get; private set; } = courseId;
}