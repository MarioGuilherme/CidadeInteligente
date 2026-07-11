using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Enums;
using FluentAssertions;
using FluentValidation.Results;

namespace CidadeInteligente.Tests.Validators;

public class CreateUserCommandValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        CreateUserCommand createUserCommand = new(1, "Valid Name User", "validEmail@gmail.com", "F4hHW#7G40,!", "F4hHW#7G40,!", Role.Student);

        // Act
        ValidationResult result = new CreateUserCommandValidator().Validate(createUserCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Invalid command should return one validation error per field")]
    public void Should_BeInvalid_WhenCommandIsInvalid()
    {
        // Arrange
        CreateUserCommand createUserCommand = new(0,
            new string('a', UserConstraints.NameMaxLength + 1),
            "invalidEmail",
            "1234567",
            "7654321",
            (Role)99);

        // Act
        ValidationResult result = new CreateUserCommandValidator().Validate(createUserCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(6);
        result.Errors.Count(e => e.ErrorMessage == "It is necessary to specify the user course").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == $"The user name cannot exceed {UserConstraints.NameMaxLength} characters").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "The user email is not valid").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == $"The password must have at least {UserConstraints.RawPasswordMinLength} characters").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "The passwords do not match").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "You must specify a permission for the user!").Should().Be(1);
    }

}
