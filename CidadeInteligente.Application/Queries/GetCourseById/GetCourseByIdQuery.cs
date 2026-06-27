using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public record GetCourseByIdQuery(long CourseId) : IRequest<GetCourseByIdQueryResult> { }
