namespace Baltaio.Location.Api.Domain.Users;

public sealed class User
{
    private static string _salt = "$2a$12$uKcRY5YbGhvTe3M0jUnJvu";

    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = BCrypt.Net.BCrypt.HashPassword(inputKey: password, _salt);
    }

    public Guid Id { get; init;  }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public bool VerifyPassword(string password) =>
        BCrypt.Net.BCrypt.Verify(password, Password);
}
