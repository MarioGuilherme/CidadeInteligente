using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Commands;

public class CreateProjectCommandHandlerTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IFileStorage> _fileStorage = new();
    private readonly CreateProjectCommandHandler _commandHandler;

    public CreateProjectCommandHandlerTests()
        => _commandHandler = new(_notificationContext.Object, _unitOfWork.Object, _fileStorage.Object);

    private static CreateProjectCommand CreateValidCommand() => new(
        CurrentUserId: 1,
        Title: "Cidade Inteligente - Monitoramento de Ar",
        AreaId: 1,
        CourseId: 1,
        Description: "Projeto de monitoramento da qualidade do ar",
        StartedAt: _today.AddDays(-10),
        FinishedAt: _today.AddDays(-1),
        InvolvedUsers: [2],
        Medias: [new("Foto do protótipo", null, "image/png", 1024, () => new MemoryStream())]);

    private void SetupTransactionToInvokeOperation() => _unitOfWork
        .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<Func<CancellationToken, Task>?>(), It.IsAny<CancellationToken>()))
        .Returns<Func<CancellationToken, Task>, Func<CancellationToken, Task>?, CancellationToken>(async (operation, _, cancellationToken) =>
        {
            await operation(cancellationToken);
            return 1;
        });

    [Fact(DisplayName = "Should create project with medias and involved users when command is valid")]
    public async Task Should_CreateProjectWithMediasAndInvolvedUsers_WhenCommandIsValid()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand();
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Courses.AnyBySpecAsync(It.IsAny<Specification<Course>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Users.GetBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User(2));
        _unitOfWork
            .Setup(u => u.Projects.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _fileStorage
            .Setup(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploadedFileName");
        SetupTransactionToInvokeOperation();

        // Act
        int? projectId = await _commandHandler.Handle(createProjectCommand, CancellationToken.None);

        // Assert
        projectId.Should().NotBeNull();
        _fileStorage.Verify(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Projects.AddAsync(It.Is<Project>(p =>
            p.Title == createProjectCommand.Title &&
            p.AreaId == createProjectCommand.AreaId &&
            p.CourseId == createProjectCommand.CourseId &&
            p.CreatedByUserId == createProjectCommand.CurrentUserId &&
            p.InvolvedUsers.Count == 1 &&
            p.Medias.Count == 1), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when area does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenAreaDoesNotExist()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand();
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWork
            .Setup(u => u.Courses.AnyBySpecAsync(It.IsAny<Specification<Course>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _notificationContext
            .SetupGet(n => n.HasNotifications)
            .Returns(true);

        // Act
        int? projectId = await _commandHandler.Handle(createProjectCommand, CancellationToken.None);

        // Assert
        projectId.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.AreaNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Projects.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
        _fileStorage.Verify(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should notify and skip involved user when involved user does not exist")]
    public async Task Should_NotifyAndSkipInvolvedUser_WhenInvolvedUserDoesNotExist()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand();
        _unitOfWork
            .Setup(u => u.Areas.AnyBySpecAsync(It.IsAny<Specification<Area>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Courses.AnyBySpecAsync(It.IsAny<Specification<Course>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _unitOfWork
            .Setup(u => u.Users.GetBySpecAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _unitOfWork
            .Setup(u => u.Projects.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _fileStorage
            .Setup(f => f.UploadOrUpdateFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploadedFileName");
        SetupTransactionToInvokeOperation();

        // Act
        int? projectId = await _commandHandler.Handle(createProjectCommand, CancellationToken.None);

        // Assert
        projectId.Should().NotBeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.UserNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Projects.AddAsync(It.Is<Project>(p => p.InvolvedUsers.Count == 0 && p.Medias.Count == 1), It.IsAny<CancellationToken>()), Times.Once);
    }
}
