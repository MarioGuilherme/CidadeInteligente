using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourseCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        Specification<Course> specCourse = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId == request.CourseId)
            .AsEditable()
            .Build();

        Course? course = await _unitOfWork.Courses.GetBySpecAsync(specCourse);
        if (course is null)
        {
            _notification.AddNotification(NotificationType.CourseNotFound);
            return null;
        }

        Specification<Course> specCourseDescriptionInUse = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId != request.CourseId && c.Description == request.Description)
            .Build();

        if (await _unitOfWork.Courses.AnyBySpecAsync(specCourseDescriptionInUse))
        {
            _notification.AddNotification(NotificationType.CourseAlreadyExists, [request.Description]);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => course.Update(request.Description), cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
