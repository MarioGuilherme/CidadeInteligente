using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetOrRemoveUserTokenRecoverPasswordCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork)
    : IRequestHandler<GetOrRemoveUserTokenRecoverPasswordCommand, GetOrRemoveUserTokenRecoverPasswordCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetOrRemoveUserTokenRecoverPasswordCommandResult?> Handle(GetOrRemoveUserTokenRecoverPasswordCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByTokenSpec = UserSpecifications.GetByToken(request.Token).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByTokenSpec, cancellationToken);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserWithTokenNotFound);
            return null;
        }

        if (DateTime.UtcNow > user.TokenRecoverPasswordExpiration)
        {
            await _unitOfWork.ExecuteInTransactionAsync(user.RemoveTokenInformations, cancellationToken: cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        return new(user.Name, user.TokenRecoverPassword!);
    }
}
