using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourseCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        Course course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId, true) ?? throw new CourseNotExistException();

        course.Update(request.Description);

        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}