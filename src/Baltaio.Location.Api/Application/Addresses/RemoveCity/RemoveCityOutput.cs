namespace Baltaio.Location.Api.Application.Addresses.RemoveCity;

public record RemoveCityOutput(bool IsValid, IEnumerable<string> Errors)
{
    public static RemoveCityOutput ValidationError(IEnumerable<string> errors) => new(false, errors);
    public static RemoveCityOutput Success() => new(true, Enumerable.Empty<string>());
}
