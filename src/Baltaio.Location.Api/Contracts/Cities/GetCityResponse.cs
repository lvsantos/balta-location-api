using Baltaio.Location.Api.Application.Addresses.GetAddress;

namespace Baltaio.Location.Api.Contracts.Cities
{
    public record GetCityResponse(
        int IbgeCode,
        string CityName,
        int StateCode,
        string StateAbreviation,
        string Statename)
    {
        public static GetCityResponse Create(GetCityOutput output) =>
            new(
                IbgeCode: output.IbgeCode!.Value,
                CityName: output.CityName,
                StateCode: output.StateCode!.Value,
                StateAbreviation: output.StateAbreviation,
                Statename: output.StateName);
    }

}
