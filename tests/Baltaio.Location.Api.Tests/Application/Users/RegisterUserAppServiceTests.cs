using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Application.Users.Register;
using Baltaio.Location.Api.Domain.Users;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Users;

public class RegisterUserAppServiceTests
{
    private RegisterUserAppService _service;
    private readonly IUserRepository _userRepository;

    public RegisterUserAppServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _service = new(_userRepository);
    }

    [Theory(DisplayName = "Deve retornar erros de validação quando o email for inválido")]
    [InlineData("invalid_email")]
    [InlineData("@")]
    [InlineData("@domain.com")]
    [InlineData("invalid_email@")]
    [InlineData("invalid_email@domain")]
    [InlineData("invalid_email@domain.")]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_EmailIsInvalid(string email)
    {
        // Arrange
        string password = "12345678";
        RegisterUserInput input = new(email, password);

        // Act
        RegisterUserOutput result = await _service.ExecuteAsync(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Should().Be("O email informado é inválido");
    }
    [Theory(DisplayName = "Deve retornar erros de validação quando a senha for inválida")]
    [InlineData("1234567")]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_PasswordIsInvalid(string password)
    {
        // Arrange
        string email = "email-valid@domain.com";
        RegisterUserInput input = new(email, password);

        // Act
        RegisterUserOutput result = await _service.ExecuteAsync(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Should().Be("A senha deve conter no mínimo 8 caracteres");
    }
    [Fact(DisplayName = "Deve retornar erros de validação quando o email e a senha forem inválidos")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationErrors_When_EmailAndPasswordAreInvalid()
    {
        // Arrange
        string email = "invalid_email";
        string password = "1234567";
        RegisterUserInput input = new(email, password);

        // Act
        RegisterUserOutput result = await _service.ExecuteAsync(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain("O email informado é inválido");
        result.Errors.Should().Contain("A senha deve conter no mínimo 8 caracteres");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando o email já existir no sistema")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_EmailAlreadyExists()
    {
        // Arrange
        string email = "valid_already_exists@domaim.com";
        string password = "12345678";
        RegisterUserInput input = new(email, password);
        _userRepository
            .ExistsAsync(email)
            .Returns(true);
        _service = new(_userRepository);

        // Act
        RegisterUserOutput result = await _service.ExecuteAsync(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Should().Be("O email informado já está em uso");
    }
    [Fact(DisplayName = "Deve retornar sucesso quando o email e a senha forem válidos")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnSuccess_When_EmailAndPasswordAreValid()
    {
        // Arrange
        string email = "valid-email@domain.com";
        string password = "12345678";
        RegisterUserInput input = new(email, password);

        //Act
        RegisterUserOutput result = await _service.ExecuteAsync(input);

        //Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _userRepository.Received(1).SaveAsync(Arg.Any<User>());
    }
}
