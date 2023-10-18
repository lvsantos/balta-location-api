using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Users.Register;

public sealed class RegisterUserInput : Notifiable<Notification>
{
    public RegisterUserInput(string email, string password)
    {
        Email = email;
        Password = password;

        AddNotifications(new RegisterUserInputValidation(this));
    }

    public string Email { get; init; }
    public string Password { get; init; }
}