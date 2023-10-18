using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Users.Register;

public record RegisterUserOutput(bool IsValid, IEnumerable<string> Errors)
{
    public static RegisterUserOutput Success() => 
        new(true, Enumerable.Empty<string>());
    public static RegisterUserOutput ValidationErrors(IEnumerable<string> errors) => 
        new(false, errors);
    public static RegisterUserOutput ValidationErrors(IEnumerable<Notification> notifications) => 
        new(false, notifications.Select(n => n.Message));
}
