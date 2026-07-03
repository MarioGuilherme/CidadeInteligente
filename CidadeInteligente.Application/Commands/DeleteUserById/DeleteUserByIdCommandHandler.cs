using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        Specification<Project> specProjectsFromUser = SpecificationBuilder<Project>.Create()
            .Where(p => p.CreatedByUserId == request.UserId || p.InvolvedUsers.Any(iu => iu.UserId == request.UserId))
            .Build();

        if (await _unitOfWork.Projects.AnyBySpecAsync(specProjectsFromUser))
        {
            _notification.AddNotification(NotificationType.UserWithDependentProjects, [request.UserId]);
            return null;
        }

        int deleted = await _unitOfWork.Users.DeleteByIdAsync(request.UserId, cancellationToken);
        if (deleted == 0)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        return Unit.Value;
    }
}
