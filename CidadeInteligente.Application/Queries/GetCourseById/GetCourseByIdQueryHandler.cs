using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetCourseByIdQuery, GetCourseByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCourseByIdQueryResult?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        Course? course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
        if (course is null)
        {
            Log.Warning("Course with ID {CourseId} ​​not found.", request.CourseId);
            _notification.AddNotification(NotificationType.CourseNotFound);
            return null;
        }

        return new(course.CourseId, course.Description);
    }
}
