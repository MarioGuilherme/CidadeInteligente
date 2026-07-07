using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CidadeInteligente.Application.Queries.AuthenticateUser;

public class AuthenticateUserQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<AuthenticateUserQuery, AuthenticateUserQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<AuthenticateUserQueryResult?> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        Specification<User, UserReadModel?> getUserByEmailSpec = UserSpecifications.GetByEmailAndExceptUserId(request.Email)
            .WithProjection<UserReadModel>(u => new(u.UserId, u.Password, u.Role)!);

        UserReadModel? possibleUser = await _unitOfWork.Users.GetBySpecAsync(getUserByEmailSpec, cancellationToken);
        if (possibleUser is null || !_passwordHasher.Verify(request.Password, possibleUser.Password))
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
