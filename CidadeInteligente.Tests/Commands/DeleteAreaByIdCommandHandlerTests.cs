using CidadeInteligente.Application.Commands.DeleteAreaById;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace CidadeInteligente.Tests.Commands;

public class DeleteAreaByIdCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly DeleteAreaByIdCommandHandler _commandHandler;

    public DeleteAreaByIdCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object);

    [Fact(DisplayName = "Should delete area when area exists and has no dependent projects")]
    public async Task Should_DeleteArea_WhenAreaExistsAndHasNoDependentProjects()
    {
        // Arrange
        DeleteAreaByIdCommand deleteAreaByIdCommand = new(1);
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Projects.AnyBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWork
            .Setup(u => u.Areas.DeleteByPredicateAsync(It.IsAny<Expression<Func<Area, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        Unit? result = await _commandHandler.Handle(deleteAreaByIdCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
        _unitOfWork.Verify(u => u.Areas.DeleteByPredicateAsync(It.IsAny<Expression<Func<Area, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should return null and notify when area has dependent projects")]
    public async Task Should_ReturnNullAndNotify_WhenAreaHasDependentProjects()
    {
        // Arrange
        DeleteAreaByIdCommand deleteAreaByIdCommand = new(1);
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Projects.AnyBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Unit? result = await _commandHandler.Handle(deleteAreaByIdCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.AreaWithDependentProjects, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Areas.DeleteByPredicateAsync(It.IsAny<Expression<Func<Area, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
