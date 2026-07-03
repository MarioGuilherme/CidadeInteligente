using CidadeInteligente.Application.Queries.GetUserById;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Queries.AuthenticateUser;

public class AuthenticateUserQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<AuthenticateUserQuery, AuthenticateUserQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthenticateUserQueryResult?> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        Specification<User, UserReadModel?> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.Email == request.Email)
            .WithProjection(u => new UserReadModel(u.UserId, u.Password, u.Role));

        UserReadModel? possibleUser = await _unitOfWork.Users.GetBySpecAsync(spec);
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

    #region ReadModel
    private sealed record UserReadModel(int UserId, string Password, Role Role);
    #endregion
}
