using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourse;

public class GetAllCoursesQuery : IRequest<List<Course>> { }