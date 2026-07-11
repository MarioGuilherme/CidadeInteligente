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
        result.Errors.Count(e => e.ErrorMessage == "É necessário especificar o curso do usuário").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == $"O nome do usuário não pode exceder {UserConstraints.NameMaxLength} caracteres").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "O e-mail do usuário não é válido").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == $"A senha deve ter pelo menos {UserConstraints.RawPasswordMinLength} caracteres").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "As senhas não conferem").Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == "É necessário especificar uma permissão para o usuário!").Should().Be(1);
    }

}
