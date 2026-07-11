using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Validators;
using CidadeInteligente.Domain.Constants;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class ChangePasswordCommandValidatorTests
{
    private static readonly string _validToken = new('a', UserConstraints.TokenRecoverPasswordMaxLength);

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        ChangePasswordCommand changePasswordCommand = new("F4hHW#7G40,!", "F4hHW#7G40,!", _validToken);

        // Act
        ValidationResult result = new ChangePasswordCommandValidator().Validate(changePasswordCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Token with wrong length should return token validation error")]
    public void Should_HaveTokenError_WhenTokenLengthIsWrong()
    {
        // Arrange
        ChangePasswordCommand changePasswordCommand = new("F4hHW#7G40,!", "F4hHW#7G40,!", "shortToken");

        // Act
        ValidationResult result = new ChangePasswordCommandValidator().Validate(changePasswordCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Count(e => e.ErrorMessage == ValidationMessages.User.TokenLength).Should().Be(1);
    }
}
