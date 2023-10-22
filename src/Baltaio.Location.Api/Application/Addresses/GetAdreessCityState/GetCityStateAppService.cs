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
       
        public async Task<List<GetCityStateOutput>> ExecuteAsync(string cityName, string stateName)
        {
            List<City> city = await _cityRepository.GetByStateOrCityAsync(cityName, stateName);

            return GetCityStateOutput.Success(city) ;
        }
    }
}
