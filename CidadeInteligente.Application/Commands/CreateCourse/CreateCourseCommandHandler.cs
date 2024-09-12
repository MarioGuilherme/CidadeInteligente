using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCourseCommand, long> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<long> Handle(CreateCourseCommand request, CancellationToken cancellationToken) {
        Course course = new(request.Description);

        await this._unitOfWork.Courses.AddAsync(course);
        await this._unitOfWork.CompleteAsync();

        return course.CourseId;
    }
}