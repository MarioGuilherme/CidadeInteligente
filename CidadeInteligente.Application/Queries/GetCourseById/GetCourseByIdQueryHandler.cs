using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetCourseByIdQuery, GetCourseByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCourseByIdQueryResult?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Course, GetCourseByIdQueryResult?> spec = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId == request.CourseId)
            .WithProjection(c => new GetCourseByIdQueryResult(c.CourseId, c.Description));

        GetCourseByIdQueryResult? course = await _unitOfWork.Courses.GetBySpecAsync(spec);
        if (course is null)
        {
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);
            return null;
        }

        return course;
    }
}
