using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Users.Login;

public record LoginOutput(string Token, bool IsValid, IEnumerable<string> Errors)
{
    public static LoginOutput ValidationErrors(IEnumerable<string> errors) =>
        new(string.Empty, false, errors);
    public static LoginOutput ValidationErrors(IEnumerable<Notification> notifications) =>
        new(string.Empty, false, notifications.Select(n => n.Message));
    public static LoginOutput Success(Token token) =>
        new(token.Value, true, Enumerable.Empty<string>());
}
