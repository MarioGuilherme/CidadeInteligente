using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Projects;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        Specification<Project> getProjectsFromUserSpec = ProjectSpecifications.GetRelatedProjectsFromUser(request.UserId).Build();
        if (await _unitOfWork.Projects.AnyBySpecAsync(getProjectsFromUserSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.UserWithDependentProjects, [request.UserId]);
            return null;
        }

        int deleted = await _unitOfWork.Users.DeleteByPredicateAsync(u => u.UserId == request.UserId, cancellationToken);
        if (deleted == 0)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        return Unit.Value;
    }
}
