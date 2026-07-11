using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CidadeInteligente.Tests.Commands;

public class DeleteProjectByIdCommandHandlerTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IFileStorage> _fileStorage = new();
    private readonly Mock<ILogger<DeleteProjectByIdCommandHandler>> _logger = new();
    private readonly DeleteProjectByIdCommandHandler _commandHandler;

    public DeleteProjectByIdCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object, _fileStorage.Object, _logger.Object);

    private static Project CreateProjectWithMedias(int createdByUserId)
    {
        Project project = new(1, 1, createdByUserId, "Cidade Inteligente", null, _today.AddDays(-30), null);
        project.Medias.Add(new Media("Foto do protótipo", null, "file-1", "image/png"));
        project.Medias.Add(new Media("Vídeo demonstração", null, "file-2", "video/mp4"));
        return project;
    }

    [Fact(DisplayName = "Should delete project and its media files when user is the creator")]
    public async Task Should_DeleteProjectAndItsMediaFiles_WhenUserIsTheCreator()
    {
        // Arrange
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(1, 1);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateProjectWithMedias(createdByUserId: 1));
        _unitOfWork
            .Setup(u => u.Projects.DeleteByPredicateAsync(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        Unit? result = await _commandHandler.Handle(deleteProjectByIdCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWork.Verify(u => u.Projects.DeleteByPredicateAsync(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _fileStorage.Verify(f => f.DeleteFileAsync("file-1", It.IsAny<CancellationToken>()), Times.Once);
        _fileStorage.Verify(f => f.DeleteFileAsync("file-2", It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when user is not the creator nor involved in the project")]
    public async Task Should_ReturnNullAndNotify_WhenUserIsNotTheCreatorNorInvolved()
    {
        // Arrange
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(1, 99);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateProjectWithMedias(createdByUserId: 1));

        // Act
        Unit? result = await _commandHandler.Handle(deleteProjectByIdCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.UserNotAuthorizedToModifyProject, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Projects.DeleteByPredicateAsync(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should still delete project when media file deletion fails")]
    public async Task Should_StillDeleteProject_WhenMediaFileDeletionFails()
    {
        // Arrange
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(1, 1);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateProjectWithMedias(createdByUserId: 1));
        _fileStorage
            .Setup(f => f.DeleteFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IOException("Storage unavailable"));

        // Act
        Unit? result = await _commandHandler.Handle(deleteProjectByIdCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWork.Verify(u => u.Projects.DeleteByPredicateAsync(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }
}
