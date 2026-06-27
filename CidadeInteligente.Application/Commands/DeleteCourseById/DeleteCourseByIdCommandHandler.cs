using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseByIdCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken)
    {
        Course course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId) ?? throw new CourseNotExistException();

        if (await _unitOfWork.Courses.HaveProjectsAsync(request.CourseId))
            throw new CourseWithDepedentProjectsException();

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
