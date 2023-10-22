using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAdreessCityState
{
    public record GetCityStateOutput()
    {
        public static GetCityStateOutput Validation() =>
            new();
        public static GetCityStateOutput Success(City city) =>
            new();
    }
}