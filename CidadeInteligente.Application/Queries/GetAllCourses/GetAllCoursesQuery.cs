using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllCourses;

public class GetAllCoursesQuery : IRequest<List<CourseViewModel>> { }