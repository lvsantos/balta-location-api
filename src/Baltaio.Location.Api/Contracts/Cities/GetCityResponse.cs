using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Application.Addresses.GetAdreessCityState;

namespace Baltaio.Location.Api.Contracts.Cities
{
    public record GetCityResponse(
        int IbgeCode,
        string CityName,
        int StateCode,
        string StateAbreviation,
        string StateName)
    {
        public static GetCityResponse Create(GetCityOutput output) =>
            new(
                IbgeCode: output.IbgeCode!.Value,
                CityName: output.CityName,
                StateCode: output.StateCode!.Value,
                StateAbreviation: output.StateAbreviation,
                StateName: output.StateName);
        public static List<GetCityResponse> Create(List<GetCityStateOutput> outputs)
        {
            return outputs.Select(c =>
            new GetCityResponse(
                IbgeCode: c.IbgeCode.Value,
                CityName: c.CityName,
                StateCode: c.StateCode.Value,
                StateAbreviation: c.StateAbreviation,
                StateName: c.StateName)).ToList();
        }
            
    }
}
