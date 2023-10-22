using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Addresses.UpdateCity;

public record UpdateCityOutput(bool IsValid, IEnumerable<string> Errors)
{
    public static UpdateCityOutput Success() =>
        new(true, Enumerable.Empty<string>());
    public static UpdateCityOutput ValidationErrors(IEnumerable<string> errors) =>
        new(false, errors);
    public static UpdateCityOutput ValidationErrors(IEnumerable<Notification> notifications) =>
        new(false, notifications.Select(n => n.Message));
}