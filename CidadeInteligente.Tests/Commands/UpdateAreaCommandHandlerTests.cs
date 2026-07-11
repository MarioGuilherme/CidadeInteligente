using CidadeInteligente.Application.Commands.UpdateArea;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class UpdateAreaCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UpdateAreaCommandHandler _commandHandler;

    public UpdateAreaCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object);

    [Fact(DisplayName = "Should update area when area exists and description is available")]
    public async Task Should_UpdateArea_WhenAreaExistsAndDescriptionIsAvailable()
    {
        // Arrange
        UpdateAreaCommand updateAreaCommand = new(1, "Mobilidade Urbana");
        Area area = new(1, "Meio Ambiente");
        _unitOfWork
            .Setup(u => u.Areas.GetBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(area);
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWork
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Action>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
            .Returns<Action, Func<CancellationToken, Task>?, CancellationToken>((operation, _, _) =>
            {
                operation();
                return Task.FromResult(1);
            });

        // Act
        Unit? result = await _commandHandler.Handle(updateAreaCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        area.Description.Should().Be("Mobilidade Urbana");
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when area does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenAreaDoesNotExist()
    {
        // Arrange
        UpdateAreaCommand updateAreaCommand = new(1, "Mobilidade Urbana");
        _unitOfWork
            .Setup(u => u.Areas.GetBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Area?)null);

        // Act
        Unit? result = await _commandHandler.Handle(updateAreaCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.AreaNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Action>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}
