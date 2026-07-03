using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
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
        //Course? course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId, true);
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

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        course.Update(request.Description);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
