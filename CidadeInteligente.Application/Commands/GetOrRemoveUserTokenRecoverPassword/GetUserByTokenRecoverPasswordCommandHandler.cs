using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Courses;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, TimeProvider timeProvider) : IRequestHandler<GetUserByTokenRecoverPasswordCommand, GetUserByTokenRecoverPasswordCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<GetUserByTokenRecoverPasswordCommandResult?> Handle(GetUserByTokenRecoverPasswordCommand request, CancellationToken cancellationToken)
    {

        Specification<User> getUserByTokenSpec = UserSpecifications.GetByToken(request.Token).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByTokenSpec, cancellationToken);
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
