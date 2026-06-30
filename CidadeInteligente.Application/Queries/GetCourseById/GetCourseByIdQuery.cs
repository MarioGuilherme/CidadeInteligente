using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public record GetCourseByIdQuery(int CourseId) : IRequest<GetCourseByIdQueryResult?> { }
