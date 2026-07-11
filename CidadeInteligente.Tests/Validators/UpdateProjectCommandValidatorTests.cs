using CidadeInteligente.Application.Commands.UpdateProject;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class UpdateProjectCommandValidatorTests
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    private static UpdateProjectCommand CreateValidCommand() => new(
        ProjectId: 1,
        CurrentUserId: 1,
        Title: "Cidade Inteligente - Monitoramento de Ar",
        AreaId: 1,
        CourseId: 1,
        Description: "Projeto de monitoramento da qualidade do ar",
        StartedAt: _today.AddDays(-10),
        FinishedAt: _today.AddDays(-1),
        InvolvedUsers: [1, 2],
        Medias: [new(1, "Foto do protótipo", null, "image/png", 1024, () => Stream.Null)]);

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = CreateValidCommand();

        // Act
        ValidationResult result = new UpdateProjectCommandValidator().Validate(updateProjectCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Kept media without new file should not require mime type")]
    public void Should_BeValid_WhenKeptMediaHasNoNewFile()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = CreateValidCommand() with
        {
            Medias = [new(1, "Foto do protótipo", null, string.Empty, 0, () => Stream.Null)]
        };

        // Act
        ValidationResult result = new UpdateProjectCommandValidator().Validate(updateProjectCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Media with invalid identifier should fail validation")]
    public void Should_BeInvalid_WhenMediaIdIsInvalid()
    {
        // Arrange
        UpdateProjectCommand updateProjectCommand = CreateValidCommand() with
        {
            Medias = [new(0, "Foto do protótipo", null, "image/png", 1024, () => Stream.Null)]
        };

        // Act
        ValidationResult result = new UpdateProjectCommandValidator().Validate(updateProjectCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Count(e => e.ErrorMessage == "The media identifier is invalid").Should().Be(1);
    }
}
