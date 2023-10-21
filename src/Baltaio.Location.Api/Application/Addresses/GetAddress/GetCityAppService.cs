using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Baltaio.Location.Api.Domain.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress
{
    public class GetCityAppService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAddressRepository _addressRepository;

        public GetCityAppService(ICityRepository cityRepository, IAddressRepository addressRepository)
        {
            _cityRepository = cityRepository;
            _addressRepository = addressRepository;
        }
       
        public async Task<GetCityOutput> ExecuteAsync(int id)
        {
            City? city = await _cityRepository.GetAsync(id);

            if (city is null)
                return GetCityOutput.Validation();

            return GetCityOutput.Success(city);
        }
    }
}
