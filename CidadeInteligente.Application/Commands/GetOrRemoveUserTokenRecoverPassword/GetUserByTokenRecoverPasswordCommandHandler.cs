using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, TimeProvider timeProvider) : IRequestHandler<GetUserByTokenRecoverPasswordCommand, GetUserByTokenRecoverPasswordCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<GetUserByTokenRecoverPasswordCommandResult?> Handle(GetUserByTokenRecoverPasswordCommand request, CancellationToken cancellationToken)
    {
        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.TokenRecoverPassword == request.Token)
            .AsEditable()
            .Build();

        User? user = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserWithTokenNotFound);
            return null;
        }

        if (_timeProvider.GetUtcNow() > user.TokenRecoverPasswordExpiration)
        {
            await _unitOfWork.ExecuteInTransactionAsync(user.RemovePasswordResetTokenInformation, cancellationToken: cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        return new(user.Name, user.TokenRecoverPassword!);
    }
}
