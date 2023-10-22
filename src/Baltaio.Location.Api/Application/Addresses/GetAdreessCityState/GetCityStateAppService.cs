using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAdreessCityState
{
    public class GetCityStateAppService : IGetCityStateAppService
    {
        private readonly ICityRepository _cityRepository;

        public GetCityStateAppService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
       
        public async Task<GetCityStateOutput> ExecuteAsync(string cityName, string stateName)
        {
            City? city = await _cityRepository.GetByStateOrCityAsync(cityName, stateName);

            if (city is null)
                return GetCityStateOutput.Validation();

            return GetCityStateOutput.Success(city);
        }
    }
}
