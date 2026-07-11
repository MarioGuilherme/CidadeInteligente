using CidadeInteligente.Application.Commands.CreateArea;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class CreateAreaCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CreateAreaCommandHandler _commandHandler;

    public CreateAreaCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object);

    [Fact(DisplayName = "Should create area when description is available")]
    public async Task Should_CreateArea_WhenDescriptionIsAvailable()
    {
        // Arrange
        CreateAreaCommand createAreaCommand = new("Meio Ambiente");
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWork
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, Func<CancellationToken, Task>?, CancellationToken>(async (operation, _, cancellationToken) =>
            {
                await operation(cancellationToken);
                return 1;
            });

        // Act
        int? areaId = await _commandHandler.Handle(createAreaCommand, CancellationToken.None);

        // Assert
        areaId.Should().NotBeNull();
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
        _unitOfWork.Verify(u => u.Areas.AddAsync(It.Is<Area>(a => a.Description == "Meio Ambiente"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should return null and notify when description is already in use")]
    public async Task Should_ReturnNullAndNotify_WhenDescriptionIsAlreadyInUse()
    {
        // Arrange
        CreateAreaCommand createAreaCommand = new("Meio Ambiente");
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        int? areaId = await _commandHandler.Handle(createAreaCommand, CancellationToken.None);

        // Assert
        areaId.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.AreaAlreadyExists, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
