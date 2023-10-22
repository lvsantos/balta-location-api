using Baltaio.Location.Api.Domain;
using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Addresses.CreateCity;
public record CreateCityOutput(int? Id, bool IsValid, IEnumerable<string> Errors)
{
    public static CreateCityOutput ValidationError(IEnumerable<Notification> errors) =>
        new(null, false, errors.Select(n => n.Message));
    public static CreateCityOutput ValidationError(IEnumerable<string> errors) =>
        new(null, false, errors);
    public static CreateCityOutput Success(City city) =>
        new(city.Code, true, Enumerable.Empty<string>());
}
