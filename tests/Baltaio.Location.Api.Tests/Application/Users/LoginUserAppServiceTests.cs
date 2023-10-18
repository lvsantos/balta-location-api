using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Domain.Users;
using Baltaio.Location.Api.Infrastructure.Users.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Users;

public class LoginUserAppServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private LoginAppService _service;

    public LoginUserAppServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        JwtSettings jwtSettings = new()
        {
            ExpirationInMinutes = 1,
            Issuer = "Teste",
            Secret = "12345678901234567890123456789012"
        };
        _jwtGenerator = new JwtGenerator(Options.Create(jwtSettings));
        _service = new LoginAppService(_userRepository, _jwtGenerator);
    }

    [Theory(DisplayName = "Deve retornar erro de validação quando o email do usuário for inválido")]
    [InlineData("invalid_email")]
    [InlineData("@")]
    [InlineData("@domain.com")]
    [InlineData("invalid_email@")]
    [InlineData("invalid_email@domain")]
    [InlineData("invalid_email@domain.")]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_EmailIsInvalidAsync(string? email)
    {
        //Arrange
        LoginInput input = new(email, "12345678");

        //Act
        LoginOutput output = await _service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.First().Should().Be("O email informado é inválido.");
    }
    [Theory(DisplayName = "Deve retornar erro de validação quando a senha do usuário for vazia")]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_PasswordIsEmptyAsync(string? password)
    {
        //Arrange
        LoginInput input = new("valid-email@domain.com.br", password);

        //Act
        LoginOutput output = await _service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.First().Should().Be("A senha não pode ser vazia.");
    }
    [Fact(DisplayName = "Deve retornar erros de validações quando o email e a senha forem inválidos")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationErrors_When_EmailAndPasswordAreInvalidAsync()
    {
        //Arrange
        LoginInput input = new("invalid_email", "");

        //Act
        LoginOutput output = await _service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(2);
        output.Errors.Should().Contain("O email informado é inválido.");
        output.Errors.Should().Contain("A senha não pode ser vazia.");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando o usuário não for encontrado")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnValidationError_When_UserIsNotFoundAsync()
    {
        //Arrange
        LoginInput input = new("user-not-found@domain.com", "12345678");
        _userRepository.LoginAsync(Arg.Any<User>()).Returns((User?)null);
        _service = new(_userRepository, _jwtGenerator);

        //Act
        LoginOutput output = await _service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.First().Should().Be("Usuário não encontrado.");
    }
    [Fact(DisplayName = "Deve retornar o token jwt quando o email e senha forem válidos")]
    [Trait("Application", "Users")]
    public async Task Should_ReturnJwtToken_When_EmailAndPasswordAreValidAsync()
    {
        //Arrange
        LoginInput input = new("email-valid@domain.com", "12345678");
        _userRepository.LoginAsync(Arg.Any<User>()).Returns(new User(input.Email, input.Password));
        _service = new(_userRepository, _jwtGenerator);

        //Act
        LoginOutput output = await _service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeTrue();
        output.Errors.Should().BeEmpty();
        output.Token.Should().NotBeNullOrEmpty();
    }
}
