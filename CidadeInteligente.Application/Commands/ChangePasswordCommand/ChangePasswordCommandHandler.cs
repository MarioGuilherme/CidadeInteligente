using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
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

        if (DateTime.Now > user.TokenRecoverPasswordExpiration)
        {
            user.RemovePasswordResetTokenInformation();
            await _unitOfWork.ExecuteInTransactionAsync(user.RemovePasswordResetTokenInformation, cancellationToken: cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => user.UpdatePassword(HashPassword(request.NewPassword)), cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
