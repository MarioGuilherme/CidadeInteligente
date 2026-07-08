using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<ChangePasswordCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Unit?> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByTokenSpec = UserSpecifications.GetByToken(request.Token).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByTokenSpec, cancellationToken);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserWithTokenNotFound);
            return null;
        }

        if (DateTime.Now > user.TokenRecoverPasswordExpiration)
        {
            user.RemoveTokenInformations();
            await _unitOfWork.ExecuteInTransactionAsync(user.RemoveTokenInformations, cancellationToken: cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => user.UpdatePasswordAndRemoveTokenInformations(_passwordHasher.Hash(request.NewPassword)), cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
