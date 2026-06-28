using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
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
        User? user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user is null)
        {
            Log.Warning("User with ID {UserId} not found.", request.UserId);
            _notification.AddNotification(NotificationType.UserNotFound);
            return null;
        }

        if (await _unitOfWork.Users.IsInvolvedOrCreatedProjectsAsync(request.UserId))
        {
            Log.Warning("User with ID {UserId} has dependent projects and cannot be deleted.", request.UserId);
            _notification.AddNotification(NotificationType.UserWithDependentProjects);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.Users.Delete(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
