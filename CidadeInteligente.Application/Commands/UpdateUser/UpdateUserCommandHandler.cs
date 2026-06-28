using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(request.UserId, true);
        if (user is null)
        {
            Log.Warning("User with ID {UserId} not found.", request.UserId);
            _notification.AddNotification(NotificationType.UserNotFound);
            return null;
        }

        if (await _unitOfWork.Users.IsEmailInUseAsync(request.Email, user.UserId, cancellationToken))
        {
            Log.Warning("Email {Email} is already in use by another user.", request.Email);
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        user.Update(request.CourseId, request.Name, request.Email, request.Role);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
