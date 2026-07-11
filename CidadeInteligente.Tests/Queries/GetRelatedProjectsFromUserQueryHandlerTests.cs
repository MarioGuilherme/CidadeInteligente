using CidadeInteligente.Application.Options;
using CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;
using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class GetRelatedProjectsFromUserQueryHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetRelatedProjectsFromUserQueryHandler _queryHandler;

    public GetRelatedProjectsFromUserQueryHandlerTests()
        => _queryHandler = new(_notificationContext.Object,
            _unitOfWork.Object,
            Options.Create(new FileStorageOptions { BaseUrl = "https://storage.cidadeinteligente.com" }),
            Options.Create(new PaginationOptions { MaxPageSize = 6 }));

    [Fact(DisplayName = "Should return paged related projects when user exists")]
    public async Task Should_ReturnPagedRelatedProjects_WhenUserExists()
    {
        // Arrange
        GetRelatedProjectsFromUserQuery getRelatedProjectsFromUserQuery = new(1, 1);
        List<GetRelatedProjectsFromUserQueryResult.ProjectViewModel> projects = [
            new(1, "Projeto Criado Pelo Usuário", null, [new(1, "https://storage.cidadeinteligente.com/file-1", "image/png")]),
            new(2, "Projeto Em Que Está Envolvido", null, [])];
        _unitOfWork
            .Setup(u => u.Users.AnyBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Projects.GetPagedBySpecAsync(It.IsAny<Specification<Project, GetRelatedProjectsFromUserQueryResult.ProjectViewModel>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<GetRelatedProjectsFromUserQueryResult.ProjectViewModel>(projects, totalItems: 2, page: 1, pageSize: 6));

        // Act
        GetRelatedProjectsFromUserQueryResult? result = await _queryHandler.Handle(getRelatedProjectsFromUserQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.ItemsCount.Should().Be(2);
        result.Data.Should().BeEquivalentTo(projects);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when user does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenUserDoesNotExist()
    {
        // Arrange
        GetRelatedProjectsFromUserQuery getRelatedProjectsFromUserQuery = new(1, 1);
        _unitOfWork
            .Setup(u => u.Users.AnyBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        GetRelatedProjectsFromUserQueryResult? result = await _queryHandler.Handle(getRelatedProjectsFromUserQuery, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.UserNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Projects.GetPagedBySpecAsync(It.IsAny<Specification<Project, GetRelatedProjectsFromUserQueryResult.ProjectViewModel>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
