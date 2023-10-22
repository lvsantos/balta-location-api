using Baltaio.Location.Api.Application.Addresses.UpdateCity;

namespace Baltaio.Location.Api.Contracts.Cities;

public record UpdateCityRequest(int IbgeCode, string Name, int StateCode)
{
    internal UpdateCityInput ToInput() => new(IbgeCode, Name, StateCode);
}
