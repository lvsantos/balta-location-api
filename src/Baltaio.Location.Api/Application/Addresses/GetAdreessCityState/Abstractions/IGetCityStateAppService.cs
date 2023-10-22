using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Application.Addresses.GetAdreessCityState;

public interface IGetCityStateAppService
{
    Task<List<GetCityStateOutput>> ExecuteAsync(string cityName, string stateName);
}

