using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken)
    {
        Course? course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
        if (course is null)
        {
            Log.Warning("Course with ID {CourseId} ​​not found.", request.CourseId);
            _notification.AddNotification(NotificationType.CourseNotFound);
            return null;
        }

        if (await _unitOfWork.Courses.HaveProjectsAsync(request.CourseId))
        {
            Log.Warning("Course with ID {CourseId} has dependent projects and cannot be deleted.", request.CourseId);
            _notification.AddNotification(NotificationType.CourseWithDependentProjects);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
