using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Users.Login;

public sealed class LoginInput : Notifiable<Notification>
{
    public LoginInput(string email, string password)
    {
        Email = email;
        Password = password;

        AddNotifications(new LoginInputValidation(this));
    }

    public string Email { get; init;  }
    public string Password { get; init; }
}
