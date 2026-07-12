using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Application.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class CreateProjectCommandValidatorTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private static CreateProjectCommand CreateValidCommand() => new(
        CurrentUserId: 1,
        Title: "Cidade Inteligente - Monitoramento de Ar",
        AreaId: 1,
        CourseId: 1,
        Description: "Projeto de monitoramento da qualidade do ar",
        StartedAt: _today.AddDays(-10),
        FinishedAt: _today.AddDays(-1),
        InvolvedUsers: [1, 2],
        Medias: [new("Foto do protótipo", "Protótipo montado", "image/png", 1024, () => Stream.Null)]);

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand();

        // Act
        ValidationResult result = new CreateProjectCommandValidator().Validate(createProjectCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Finish date before start date should fail validation")]
    public void Should_BeInvalid_WhenFinishDateIsBeforeStartDate()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand() with
        {
            StartedAt = _today.AddDays(-5),
            FinishedAt = _today.AddDays(-10)
        };

        // Act
        ValidationResult result = new CreateProjectCommandValidator().Validate(createProjectCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.Project.FinishedAtBeforeStartedAt).Should().Be(1);
    }

    [Fact(DisplayName = "Project without involved users should fail validation")]
    public void Should_BeInvalid_WhenThereAreNoInvolvedUsers()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand() with { InvolvedUsers = [] };

        // Act
        ValidationResult result = new CreateProjectCommandValidator().Validate(createProjectCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.Project.InvolvedUsersRequired).Should().Be(1);
    }

    [Fact(DisplayName = "Media with unsupported mime type should fail validation")]
    public void Should_BeInvalid_WhenMediaMimeTypeIsNotSupported()
    {
        // Arrange
        CreateProjectCommand createProjectCommand = CreateValidCommand() with
        {
            Medias = [new("Documento", null, "application/pdf", 1024, () => Stream.Null)]
        };

        // Act
        ValidationResult result = new CreateProjectCommandValidator().Validate(createProjectCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.Media.MimeTypeNotSupported).Should().Be(1);
    }

    // CreateProjectCommandValidatorTests
    [Fact(DisplayName = "Media with empty file should fail validation")]
    public void Should_BeInvalid_WhenMediaFileIsEmpty()
    {
        // Arrange
        CreateProjectCommand command = CreateValidCommand() with
        {
            Medias = [new("Foto do protótipo", null, "image/png", 0, () => Stream.Null)]
        };

        // Act
        ValidationResult result = new CreateProjectCommandValidator().Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.Media.FileRequired).Should().Be(1);
    }
}
