using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourses;

public record GetCoursesQuery : IRequest<GetCoursesQueryResult> { }
