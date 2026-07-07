using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Courses;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourseCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        Specification<Course> getCourseByIdSpec = CourseSpecifications.GetById(request.CourseId).Build();
        Course? course = await _unitOfWork.Courses.GetBySpecAsync(getCourseByIdSpec, cancellationToken);
        if (course is null)
        {
            _notification.AddNotification(NotificationType.CourseNotFound);
            return null;
        }

        Specification<Course> specCourseDescriptionInUse = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId != request.CourseId && c.Description == request.Description)
            .Build();
        if (await _unitOfWork.Courses.AnyBySpecAsync(specCourseDescriptionInUse, cancellationToken))
        {
            _notification.AddNotification(NotificationType.CourseAlreadyExists, [request.Description]);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => course.Update(request.Description), cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
