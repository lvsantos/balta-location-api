namespace Baltaio.Location.Api.Domain.Users;

public sealed class User
{
    private User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }

    public Guid Id { get; init;  }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public static User Create(string email, string password, string salt) =>
        new(email, BCrypt.Net.BCrypt.HashPassword(inputKey: password, salt));

    public bool VerifyPassword(string password) =>
        BCrypt.Net.BCrypt.Verify(password, Password);
}
