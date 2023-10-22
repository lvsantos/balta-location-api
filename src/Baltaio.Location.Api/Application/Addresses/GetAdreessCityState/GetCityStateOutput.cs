using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAdreessCityState
{
    public record GetCityStateOutput(int? IbgeCode, string NameCity, string StateCode, bool Valid)
    {
        public static GetCityStateOutput Validation() =>
            new(null, string.Empty, string.Empty, false);
        public static GetCityStateOutput Success(City city) =>
            new(city.Code, city.Name, city.State.Abbreviation, true);
    }
}