using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress
{
    public record GetCityOutput(int? IbgeCode, string NameCity, int? StateCode, bool Valid, string Message)
    {
        public static GetCityOutput Validation() =>
            new(null, string.Empty, null, false, "Id não encontrado");
        public static GetCityOutput Success(City city) =>
            new(city.IbgeCode, city.NameCity, city.StateCode, true, string.Empty);
    }
}