using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetUserByTokenRecoverPasswordQuery, GetUserByTokenRecoverPasswordQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByTokenRecoverPasswordQueryResult?> Handle(GetUserByTokenRecoverPasswordQuery request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.Users.GetByTokenRecoverPasswordAsync(request.Token);
        if (user is null)
        {
            Log.Warning("User ​​not found during password recovery.", request.Token);
            _notification.AddNotification(NotificationType.UserWithTokenNotFound);
            return null;
        }

        if (DateTime.Now > user.TokenRecoverPasswordExpiration)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            user.RemovePasswordResetTokenInformation();
            await _unitOfWork.CommitAsync(cancellationToken);
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        return new(user.Name, user.TokenRecoverPassword!);
    }
}
