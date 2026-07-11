using CidadeInteligente.Application.Queries.AuthenticateUser;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class AuthenticateUserQueryHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly AuthenticateUserQueryHandler _queryHandler;

    public AuthenticateUserQueryHandlerTests()
        => _queryHandler = new(_notificationContext.Object, _unitOfWork.Object, _passwordHasher.Object);

    [Fact(DisplayName = "Should return null and notify invalid credentials when user does not exist")]
    public async Task Should_ReturnNullAndNotifyInvalidCredentials_WhenUserDoesNotExist()
    {
        // Arrange
        AuthenticateUserQuery authenticateUserQuery = new("validEmail@gmail.com", "F4hHW#7G40,!");
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(Mock.Of<IUserRepository>());

        // Act
        AuthenticateUserQueryResult? result = await _queryHandler.Handle(authenticateUserQuery, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.InvalidLoginCredentials, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _passwordHasher.Verify(p => p.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
