using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateCourseCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreateCourseCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        Specification<Course> specCourseDescriptionInUse = SpecificationBuilder<Course>.Create()
            .Where(a => a.Description == request.Description)
            .Build();

        if (await _unitOfWork.Courses.AnyBySpecAsync(specCourseDescriptionInUse))
        {
            _notification.AddNotification(NotificationType.CourseAlreadyExists, [request.Description]);
            return null;
        }

        Course course = new(request.Description);

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await _unitOfWork.Courses.AddAsync(course);
        }, cancellationToken: cancellationToken);

        return course.CourseId;
    }
}
