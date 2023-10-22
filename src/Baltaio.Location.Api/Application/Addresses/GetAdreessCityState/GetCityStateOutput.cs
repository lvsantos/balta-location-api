using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAdreessCityState
{
    public record GetCityStateOutput(int? IbgeCode, string NameCity, int StateCode, bool Valid)
    {
        public static List<GetCityStateOutput> Success(List<City> city)
        {
            List<GetCityStateOutput> outputs = city.Select(c => new GetCityStateOutput(c.Code, c.Name, c.StateCode, true))
                .ToList();
            return outputs;
        }
    }
}