using Baltaio.Location.Api.Application.Addresses.UpdateCity;

namespace Baltaio.Location.Api.Contracts.Cities;

public record UpdateCityRequest(string Name, int StateCode)
{
    internal UpdateCityInput ToInput(int id) => new(id, Name, StateCode);
}
