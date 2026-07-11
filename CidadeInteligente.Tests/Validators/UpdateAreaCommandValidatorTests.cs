using CidadeInteligente.Application.Commands.UpdateArea;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class UpdateAreaCommandValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        UpdateAreaCommand updateAreaCommand = new(1, "Mobilidade Urbana");

        // Act
        ValidationResult result = new UpdateAreaCommandValidator().Validate(updateAreaCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Invalid identifier and empty description should return both validation errors")]
    public void Should_BeInvalid_WhenAllFieldsAreInvalid()
    {
        // Arrange
        UpdateAreaCommand updateAreaCommand = new(-1, string.Empty);

        // Act
        ValidationResult result = new UpdateAreaCommandValidator().Validate(updateAreaCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Count(e => e.ErrorMessage == "O identificador da área é inválido").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "A descrição da área é obrigatória").Should().Be(1);
    }
}
