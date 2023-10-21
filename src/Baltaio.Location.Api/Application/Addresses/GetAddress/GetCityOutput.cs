using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress
{
    public record GetCityOutput(int? IbgeCode, string NameCity, string StateCode, bool Valid, string Message)
    {
        public static GetCityOutput Validation() =>
            new(null, string.Empty, string.Empty, false, "Id não encontrado");
        public static GetCityOutput Success(City city) =>
            new(city.CityCode, city.CityName, city.StateAbbreviation, true, string.Empty);
    }
}