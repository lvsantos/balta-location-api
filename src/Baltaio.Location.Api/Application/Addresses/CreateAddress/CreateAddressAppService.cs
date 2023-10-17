using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;

public class CreateAddressAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IAddressRepository _addressRepository;

    public CreateAddressAppService(ICityRepository cityRepository, IAddressRepository addressRepository)
    {
        _cityRepository = cityRepository;
        _addressRepository = addressRepository;
    }

    public async Task<CreateAddressOutput> ExecuteAsync(CreateAddressInput input)
    {
        City? city = await _cityRepository.GetAsync(input.IbgeCode);

        if (city is null)
            return new CreateAddressOutput(Guid.Empty, "Código IBGE não encontrado.");

        Address address = new(city);

        await _addressRepository.SaveAsync(address);

        return new CreateAddressOutput(address.Code, "Endereço criado com sucesso.");
    }


}
