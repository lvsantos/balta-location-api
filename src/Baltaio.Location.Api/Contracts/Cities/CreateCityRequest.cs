namespace Baltaio.Location.Api.Contracts.Cities;

public record CreateCityRequest(int IbgeCode, string Name, int StateCode);

