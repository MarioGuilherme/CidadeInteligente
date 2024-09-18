using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCourseByIdQuery, CourseViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CourseViewModel> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken) {
        Course course = await this._unitOfWork.Courses.GetByIdAsync(request.CourseId) ?? throw new CourseNotExistException();
        return this._mapper.Map<CourseViewModel>(course);
    }
}