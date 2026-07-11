using CidadeInteligente.Application.Queries.GetAreaById;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class GetAreaByIdQueryHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetAreaByIdQueryHandler _queryHandler;

    public GetAreaByIdQueryHandlerTests()
        => _queryHandler = new(_notificationContext.Object, _unitOfWork.Object);

    [Fact(DisplayName = "Should return area when area exists")]
    public async Task Should_ReturnArea_WhenAreaExists()
    {
        // Arrange
        GetAreaByIdQuery getAreaByIdQuery = new(1);
        GetAreaByIdQueryResult expectedResult = new(1, "Meio Ambiente");
        _unitOfWork
            .Setup(u => u.Areas.GetBySpecAsync(It.IsAny<Specification<Area, GetAreaByIdQueryResult?>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        GetAreaByIdQueryResult? result = await _queryHandler.Handle(getAreaByIdQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResult);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when area does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenAreaDoesNotExist()
    {
        // Arrange
        GetAreaByIdQuery getAreaByIdQuery = new(1);
        _unitOfWork
            .Setup(u => u.Areas.GetBySpecAsync(It.IsAny<Specification<Area, GetAreaByIdQueryResult?>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetAreaByIdQueryResult?)null);

        // Act
        GetAreaByIdQueryResult? result = await _queryHandler.Handle(getAreaByIdQuery, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.AreaNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
    }
}
