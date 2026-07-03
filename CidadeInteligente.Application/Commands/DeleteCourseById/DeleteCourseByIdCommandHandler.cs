using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken)
    {
        Specification<Course> specCourse = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId == request.CourseId)
            .Build();

        if (!await _unitOfWork.Courses.AnyBySpecAsync(specCourse))
        {
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);
            return null;
        }

        Specification<Project> specProjectsFromCourse = SpecificationBuilder<Project>.Create()
            .Where(p => p.CourseId == request.CourseId)
            .Build();
        Specification<User> specUsersFromCourse = SpecificationBuilder<User>.Create()
            .Where(u => u.CourseId == request.CourseId)
            .Build();
        if (await _unitOfWork.Projects.AnyBySpecAsync(specProjectsFromCourse) || await _unitOfWork.Users.AnyBySpecAsync(specUsersFromCourse))
        {
            _notification.AddNotification(NotificationType.CourseWithDependentProjectsOrUser, [request.CourseId]);
            return null;
        }

        await _unitOfWork.Courses.DeleteByIdAsync(request.CourseId, cancellationToken);

        return Unit.Value;
    }
}
