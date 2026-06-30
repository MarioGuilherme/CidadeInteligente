using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.IsEmailInUseAsync(request.Email, cancellationToken: cancellationToken))
        {
            Log.Warning("The email address {Email} is already in use.", request.Email);
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        User user = new(request.CourseId,
            request.Name,
            request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password),
            request.Role);
        await _unitOfWork.Users.CreateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return user.UserId;
    }
}
