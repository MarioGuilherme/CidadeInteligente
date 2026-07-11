using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class UpdateProjectCommandHandlerTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IFileStorage> _fileStorage = new();
    private readonly Mock<ILogger<UpdateProjectCommandHandler>> _logger = new();
    private readonly UpdateProjectCommandHandler _commandHandler;

    public UpdateProjectCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object, _fileStorage.Object, _logger.Object);

    private void SetupTransactionToInvokeOperation() => _unitOfWork
        .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
        .Returns<Func<CancellationToken, Task>, Func<CancellationToken, Task>?, CancellationToken>(async (operation, _, cancellationToken) =>
        {
            await operation(cancellationToken);
            return 1;
        });

    [Fact(DisplayName = "Should update project, replace medias and involved users when user is the creator")]
    public async Task Should_UpdateProjectReplaceMediasAndInvolvedUsers_WhenUserIsTheCreator()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = new(
            ProjectId: 1,
            CurrentUserId: 1,
            Title: "Novo Título",
            AreaId: 2,
            CourseId: 2,
            Description: "Nova descrição",
            StartedAt: _today.AddDays(-10),
            FinishedAt: _today.AddDays(-1),
            InvolvedUsers: [2],
            Medias: [new(null, "Nova foto", null, "image/png", 1024, () => new MemoryStream())]);
        Project project = new(1, 1, 1, "Título Antigo", "Descrição antiga", _today.AddDays(-30), null);
        project.Medias.Add(new Media("Foto antiga", null, "old-file", "image/png"));
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _unitOfWork
            .Setup(u => u.Users.GetBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User(2));
        _fileStorage
            .Setup(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploadedFileName");
        SetupTransactionToInvokeOperation();

        // Act
        Unit? result = await _commandHandler.Handle(updateProjectCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        project.Title.Should().Be("Novo Título");
        project.AreaId.Should().Be(2);
        project.CourseId.Should().Be(2);
        project.Description.Should().Be("Nova descrição");
        project.InvolvedUsers.Should().ContainSingle(u => u.UserId == 2);
        project.Medias.Should().ContainSingle(m => m.Title == "Nova foto");
        _fileStorage.Verify(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        _fileStorage.Verify(f => f.DeleteFileAsync("old-file", It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should update project when user is not the creator but is involved")]
    public async Task Should_UpdateProject_WhenUserIsNotTheCreatorButIsInvolved()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = new(
            ProjectId: 1,
            CurrentUserId: 2,
            Title: "Novo Título",
            AreaId: 1,
            CourseId: 1,
            Description: null,
            StartedAt: _today.AddDays(-10),
            FinishedAt: null,
            InvolvedUsers: [2],
            Medias: []);
        Project project = new(1, 1, 1, "Título Antigo", null, _today.AddDays(-30), null);
        project.InvolvedUsers.Add(new User(2));
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        SetupTransactionToInvokeOperation();

        // Act
        Unit? result = await _commandHandler.Handle(updateProjectCommand, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        project.Title.Should().Be("Novo Título");
        project.InvolvedUsers.Should().ContainSingle(u => u.UserId == 2);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when project does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenProjectDoesNotExist()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = new(1, 1, "Novo Título", 1, 1, null, _today.AddDays(-10), null, [1], []);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        Unit? result = await _commandHandler.Handle(updateProjectCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.ProjectNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when user is not the creator nor involved in the project")]
    public async Task Should_ReturnNullAndNotify_WhenUserIsNotTheCreatorNorInvolved()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = new(1, 99, "Novo Título", 1, 1, null, _today.AddDays(-10), null, [1], []);
        Project project = new(1, 1, 1, "Título Antigo", null, _today.AddDays(-30), null);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        Unit? result = await _commandHandler.Handle(updateProjectCommand, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        project.Title.Should().Be("Título Antigo");
        _notificationContext.Verify(n => n.AddNotification(NotificationType.UserNotAuthorizedToModifyProject, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
