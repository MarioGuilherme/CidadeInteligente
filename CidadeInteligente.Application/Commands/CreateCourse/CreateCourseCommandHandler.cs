using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCourseCommand, int?>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        Course course = new(request.Description);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.CommitAsync(cancellationToken);

        return course.CourseId;
    }
}
