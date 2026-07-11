using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class ChangePasswordCommandHandlerTests
{
    private static readonly string _validToken = new('a', UserConstraints.TokenRecoverPasswordMaxLength);

    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly ChangePasswordCommandHandler _commandHandler;

    public ChangePasswordCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object, _passwordHasher.Object);

    [Fact(DisplayName = "Should change password and remove token informations when token is valid")]
    public async Task Should_ChangePasswordAndRemoveTokenInformations_WhenTokenIsValid()
    {
        // Arrange
        ChangePasswordCommand changePasswordCommand = new("F4hHW#7G40,!", "F4hHW#7G40,!", _validToken);
        User user = new(1, 1, "Valid Name User", "validEmail@gmail.com", "oldHashedPassword", Role.Student);
        _unitOfWork
            .Setup(u => u.Users.GetBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher
            .Setup(p => p.Hash(It.IsAny<string>()))
            .Returns("newHashedPassword");
        _unitOfWork
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Action>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
            .Returns<Action, Func<CancellationToken, Task>?, CancellationToken>((operation, _, _) =>
            {
                operation();
                return Task.FromResult(1);
            });

        // Act
        Unit? result = await _commandHandler.Handle(changePasswordCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        user.Password.Should().Be("newHashedPassword");
        user.TokenRecoverPassword.Should().BeNull();
        user.TokenRecoverPasswordExpiration.Should().BeNull();
        _passwordHasher.Verify(p => p.Hash("F4hHW#7G40,!"), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when no user owns the token")]
    public async Task Should_ReturnNullAndNotify_WhenNoUserOwnsTheToken()
    {
        // Arrange
        ChangePasswordCommand changePasswordCommand = new("F4hHW#7G40,!", "F4hHW#7G40,!", _validToken);
        _unitOfWork
            .Setup(u => u.Users.GetBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Unit? result = await _commandHandler.Handle(changePasswordCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.UserWithTokenNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _passwordHasher.Verify(p => p.Hash(It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Action>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
