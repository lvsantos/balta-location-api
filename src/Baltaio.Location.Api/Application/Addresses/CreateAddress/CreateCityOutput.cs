using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;
public record CreateCityOutput(int? Id, bool Valid, string Message)
{
    public static CreateCityOutput Validation() =>
        new(null, false, "Código IBGE ja existe");
    public static CreateCityOutput Success() =>
        new(null, true, string.Empty);
}
