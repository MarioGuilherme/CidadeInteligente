using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, LoginUserCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LoginUserCommandResult?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.Email == request.Email)
            .AsEditable()
            .Build();

        User? possibleUser = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (possibleUser is null || !Verify(request.Password, possibleUser.Password))
        {
            _notification.AddNotification(NotificationType.UserWithEmailNotFound);
            return null;
        }

        IEnumerable<Claim> claims = [
            new(nameof(possibleUser.UserId), possibleUser.UserId.ToString()),
            new(ClaimTypes.Role, possibleUser.Role.ToString())];
        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new(new(claimsIdentity));
    }
}
