using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.GetAddress.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress;

internal class GetCityAppService : IGetCityAppService
{
    private readonly ICityRepository _cityRepository;

    public GetCityAppService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
   
    public async Task<GetCityOutput> ExecuteAsync(GetCityInput input)
    {
        City? city = await _cityRepository.GetWithState(input.Code);

        if (city is null || city.IsRemoved)
            return GetCityOutput.Validation();

        return GetCityOutput.Success(city);
    }
}
