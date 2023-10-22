namespace Baltaio.Location.Api.Application.Addresses.GetAddress.Abstractions;

public interface IGetCityAppService
{
    Task<GetCityOutput> ExecuteAsync(GetCityInput input);
}
