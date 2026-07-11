using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly CreateUserCommandHandler _commandHandler;

    public CreateUserCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object, _passwordHasher.Object);

    [Fact(DisplayName = "Should create user with hashed password when email is available")]
    public async Task Should_CreateUserWithHashedPassword_WhenEmailIsAvailable()
    {
        // Arrange
        CreateUserCommand createUserCommand = new(1, "Valid Name User", "validEmail@gmail.com", "F4hHW#7G40,!", "F4hHW#7G40,!", Role.Student);
        _unitOfWork
            .Setup(u => u.Users.AnyBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _passwordHasher
            .Setup(p => p.Hash(It.IsAny<string>()))
            .Returns("hashedPassword");
        _unitOfWork
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, Func<CancellationToken, Task>?, CancellationToken>(async (operation, _, cancellationToken) =>
            {
                await operation(cancellationToken);
                return 1;
            });

        // Act
        int? userId = await _commandHandler.Handle(createUserCommand, CancellationToken.None);

        // Assert
        userId.Should().NotBeNull();
        _passwordHasher.Verify(p => p.Hash("F4hHW#7G40,!"), Times.Once);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.Is<User>(user => user.Password == "hashedPassword" && user.Email == "validEmail@gmail.com"), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when email is already in use")]
    public async Task Should_ReturnNullAndNotify_WhenEmailIsAlreadyInUse()
    {
        // Arrange
        CreateUserCommand createUserCommand = new(1, "Valid Name User", "validEmail@gmail.com", "F4hHW#7G40,!", "F4hHW#7G40,!", Role.Student);
        _unitOfWork
            .Setup(u => u.Users.AnyBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        int? userId = await _commandHandler.Handle(createUserCommand, CancellationToken.None);

        // Assert
        userId.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.EmailAlreadyInUse, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _passwordHasher.Verify(p => p.Hash(It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
