using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, LoginUserCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LoginUserCommandResult?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? possibleUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (possibleUser is null)
        {
            Log.Warning("User with email {Email} not found", request.Email);
            _notification.AddNotification(NotificationType.UserWithEmailNotFound);
            return null;
        }

        if (!Verify(request.Password, possibleUser.Password))
        {
            Log.Warning("Non-existent email and/or password combination.");
            _notification.AddNotification(NotificationType.InvalidLoginCredentials);
            return null;
        }

        return new(possibleUser.UserId, possibleUser.Role.GetDescription());
    }
}
