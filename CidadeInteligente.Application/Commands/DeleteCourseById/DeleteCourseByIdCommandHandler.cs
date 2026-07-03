using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
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
            .AsEditable()
            .Build();

        Course? course = await _unitOfWork.Courses.GetBySpecAsync(specCourse);
        if (course is null)
        {
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);
            return null;
        }

        Specification<Project> specProjectsFromCourse = SpecificationBuilder<Project>.Create()
            .Where(p => p.CourseId == request.CourseId)
            .Build();
        bool areaHasDependentProjects = await _unitOfWork.Projects.AnyBySpecAsync(specProjectsFromCourse);
        if (areaHasDependentProjects)
        {
            _notification.AddNotification(NotificationType.CourseWithDependentProjects, [request.CourseId]);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Courses.DeleteAsync(course);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
