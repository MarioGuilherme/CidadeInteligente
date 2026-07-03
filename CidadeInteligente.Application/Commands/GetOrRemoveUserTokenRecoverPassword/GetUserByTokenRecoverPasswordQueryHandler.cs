using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetUserByTokenRecoverPasswordQuery, GetUserByTokenRecoverPasswordQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByTokenRecoverPasswordQueryResult?> Handle(GetUserByTokenRecoverPasswordQuery request, CancellationToken cancellationToken)
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
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            user.RemovePasswordResetTokenInformation();
            await _unitOfWork.CommitAsync(cancellationToken);
            _notification.AddNotification(NotificationType.TokenRecoverPasswordExpired);
            return null;
        }

        return new(user.Name, user.TokenRecoverPassword!);
    }
}
