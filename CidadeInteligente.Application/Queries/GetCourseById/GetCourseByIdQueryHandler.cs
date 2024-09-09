using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(ICourseRepository courseRepository, IMapper mapper) : IRequestHandler<GetCourseByIdQuery, CourseViewModel?> {
    private readonly ICourseRepository _courseRepository = courseRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CourseViewModel?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken) {
        Course? course = await this._courseRepository.GetByIdAsync(request.CourseId);
        return this._mapper.Map<CourseViewModel?>(course);
    }
}