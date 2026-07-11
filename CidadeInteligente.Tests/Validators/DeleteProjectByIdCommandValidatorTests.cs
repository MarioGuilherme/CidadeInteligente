using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Application.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class DeleteProjectByIdCommandValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(1, 1);

        // Act
        ValidationResult result = new DeleteProjectByIdCommandValidator().Validate(deleteProjectByIdCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Invalid identifiers should return both validation errors")]
    public void Should_BeInvalid_WhenIdentifiersAreInvalid()
    {
        // Arrange
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(0, 0);

        // Act
        ValidationResult result = new DeleteProjectByIdCommandValidator().Validate(deleteProjectByIdCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.Project.InvalidId).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.User.InvalidId).Should().Be(1);
    }
}
