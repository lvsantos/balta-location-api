using Baltaio.Location.Api.Domain.Users;
using FluentAssertions;

namespace Baltaio.Location.Api.Tests.Domain.Users;

public class UserTests
{
    [Fact(DisplayName = "Deve criar um usuário válido")]
    public void Should_CreateValidUser()
    {
        // Arrange
        var email = "valid-email@domain.com";
        var password = "12345678";

        // Act
        User user = new(email, password);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be(email);
        user.Password.Should().NotBe(password);
    }
    [Fact(DisplayName = "Deve criar um usuário com senha com hash")]
    public void Should_CreateUserWithHashedPassword()
    {
        // Arrange
        var email = "valid-email@domain.com";
        var password = "12345678";

        // Act
        User user = new(email, password);
        User otherUser = new(email, password);

        // Assert
        user.Should().NotBeNull();
        user.VerifyPassword(password).Should().BeTrue();
        user.VerifyPassword("invalid_password").Should().BeFalse();
        user.Password.Should().Be(otherUser.Password);
    }
}
