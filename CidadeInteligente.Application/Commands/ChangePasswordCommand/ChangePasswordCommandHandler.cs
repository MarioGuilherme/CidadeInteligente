using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

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

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        if (DateTime.Now > user.TokenRecoverPasswordExpiration)
        {
            user.RemovePasswordResetTokenInformation();
            await _unitOfWork.CommitAsync(cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(request.NewPassword));
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
