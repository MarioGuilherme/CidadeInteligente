using CidadeInteligente.Application.Options;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class GetProjectByIdQueryHandlerTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetProjectByIdQueryHandler _queryHandler;

    public GetProjectByIdQueryHandlerTests()
        => _queryHandler = new(_notificationContext.Object,
            _unitOfWork.Object,
            Options.Create(new FileStorageOptions { BaseUrl = "https://storage.cidadeinteligente.com" }));

    [Fact(DisplayName = "Should return project with involved users and medias when project exists")]
    public async Task Should_ReturnProjectWithInvolvedUsersAndMedias_WhenProjectExists()
    {
        // Arrange
        GetProjectByIdQuery getProjectByIdQuery = new(1);
        GetProjectByIdQueryResult expectedResult = new(1,
            "Cidade Inteligente - Monitoramento de Ar",
            "Meio Ambiente",
            1,
            "Análise e Desenvolvimento de Sistemas",
            1,
            "Projeto de monitoramento da qualidade do ar",
            _today.AddDays(-10),
            _today.AddDays(-1),
            [new(1, "Valid Name User"), new(2, "Another Valid User")],
            [new(1, "Foto do protótipo", null, "https://storage.cidadeinteligente.com/file-1", "image/png"),
             new(2, "Vídeo demonstração", null, "https://storage.cidadeinteligente.com/file-2", "video/mp4")]);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project, GetProjectByIdQueryResult?>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        GetProjectByIdQueryResult? result = await _queryHandler.Handle(getProjectByIdQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResult);
        result.InvolvedUsers.Should().HaveCount(2);
        result.Medias.Should().HaveCount(2);
        result.Medias.Single(m => m.MimeType == "video/mp4").IsVideo.Should().BeTrue();
        result.Medias.Single(m => m.MimeType == "image/png").IsVideo.Should().BeFalse();
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and notify when project does not exist")]
    public async Task Should_ReturnNullAndNotify_WhenProjectDoesNotExist()
    {
        // Arrange
        GetProjectByIdQuery getProjectByIdQuery = new(1);
        _unitOfWork
            .Setup(u => u.Projects.GetBySpecAsync(It.IsAny<Specification<Project, GetProjectByIdQueryResult?>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetProjectByIdQueryResult?)null);

        // Act
        GetProjectByIdQueryResult? result = await _queryHandler.Handle(getProjectByIdQuery, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(NotificationType.ProjectNotFound, It.IsAny<IEnumerable<object>?>()), Times.Once);
    }
}
