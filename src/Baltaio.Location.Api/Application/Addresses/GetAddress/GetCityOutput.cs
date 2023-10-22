using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress
{
    public record GetCityOutput(int? IbgeCode, string NameCity, int? StateCode, bool Valid, string Message)
    {
        public static GetCityOutput Validation() =>
            new(null, string.Empty, null, false, "Id não encontrado");
        public static GetCityOutput Success(City city) =>
            new(city.Code, city.Name, null, true, string.Empty);
    }
}