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
   
    public async Task<GetCityOutput> ExecuteAsync(int id)
    {
        City? city = await _cityRepository.GetWithState(id);

        if (city is null)
            return GetCityOutput.Validation();

        return GetCityOutput.Success(city);
    }
}
