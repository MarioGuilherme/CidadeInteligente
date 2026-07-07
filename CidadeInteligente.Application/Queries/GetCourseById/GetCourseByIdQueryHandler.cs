using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Courses;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetCourseByIdQuery, GetCourseByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCourseByIdQueryResult?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Course, GetCourseByIdQueryResult?> getCourseByIdSpec = CourseSpecifications.GetById(request.CourseId)
            .WithProjection<GetCourseByIdQueryResult>(c => new(c.CourseId, c.Description));

        GetCourseByIdQueryResult? course = await _unitOfWork.Courses.GetBySpecAsync(getCourseByIdSpec, cancellationToken);
        if (course is null)
        {
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);
            return null;
        }

        return course;
    }
}
