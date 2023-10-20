namespace Baltaio.Location.Api.Domain.Users;

public sealed class User
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public Guid Id { get; init;  }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public bool VerifyPassword(string password) =>
        BCrypt.Net.BCrypt.Verify(password, Password);
}
