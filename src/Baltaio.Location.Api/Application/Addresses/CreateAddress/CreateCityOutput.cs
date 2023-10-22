using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;
public record CreateCityOutput(int? Id, bool IsValid, IEnumerable<string> Errors)
{
    public static CreateCityOutput ValidationError(IEnumerable<string> errors) =>
        new(null, false, errors);
    public static CreateCityOutput Success(City city) =>
        new(city.Code, true, Enumerable.Empty<string>());
}
