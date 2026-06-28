using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourseCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        Course? course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId, true);
        if (course is null)
        {
            Log.Warning("Course with ID {CourseId} ​​not found.", request.CourseId);
            _notification.AddNotification(NotificationType.CourseNotFound);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        course.Update(request.Description);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
