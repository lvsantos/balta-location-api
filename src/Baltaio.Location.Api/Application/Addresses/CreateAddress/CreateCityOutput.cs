namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;
public record CreateCityOutput(int? Id, bool Valid, string Message)
{
    public static CreateCityOutput Validation() =>
        new(null, false, "Código IBGE ja existe");
    public static CreateCityOutput Success() =>
        new(null, true, string.Empty);
}
