using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;

public class CreateCityAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IAddressRepository _addressRepository;

    public CreateCityAppService(ICityRepository cityRepository, IAddressRepository addressRepository)
    {
        _cityRepository = cityRepository;
        _addressRepository = addressRepository;
    }

    public async Task<CreateCityOutput> ExecuteAsync(CreateCityInput input)
    {
        City? city = await _cityRepository.GetAsync(input.IbgeCode);
        if (city is not null)
            return  CreateCityOutput.Validation();

        await _cityRepository.SaveAsync(input.IbgeCode, input.NameCity, input.StateCode);

        return CreateCityOutput.Success();        
    }
}
