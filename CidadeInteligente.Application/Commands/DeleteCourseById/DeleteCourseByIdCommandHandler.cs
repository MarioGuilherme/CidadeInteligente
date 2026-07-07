using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Courses;
using CidadeInteligente.Domain.Specifications.Projects;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken)
    {
        Specification<Course> getCourseByIdSpec = CourseSpecifications.GetById(request.CourseId).Build();
        if (!await _unitOfWork.Courses.AnyBySpecAsync(getCourseByIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);
            return null;
        }

        Specification<Project> getProjectsByCourseIdSpec = ProjectSpecifications.GetByCourseId(request.CourseId).Build();
        Specification<User> getUsersByCourseIdSpec = UserSpecifications.GetByCourseId(request.CourseId).Build();
        if (await _unitOfWork.Projects.AnyBySpecAsync(getProjectsByCourseIdSpec, cancellationToken) || await _unitOfWork.Users.AnyBySpecAsync(getUsersByCourseIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.CourseWithDependentProjectsOrUser, [request.CourseId]);
            return null;
        }

        await _unitOfWork.Courses.DeleteByPredicateAsync(c => c.CourseId == request.CourseId, cancellationToken);

        return Unit.Value;
    }
}
