using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.CreateAddress;

public class CreateCityAppService : ICreateCityAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _stateRepository;

    public CreateCityAppService(ICityRepository cityRepository, IStateRepository stateRepository)
    {
        _cityRepository = cityRepository;
        _stateRepository = stateRepository;
    }

    public async Task<CreateCityOutput> ExecuteAsync(CreateCityInput input)
    {
        City? city = await _cityRepository.GetAsync(input.IbgeCode);
        if (city is not null)
            return  CreateCityOutput.Validation();


        State? state = await _stateRepository.GetAsync(input.StateCode);
        if(state is null)
            return CreateCityOutput.Validation();

        City newCity = new(input.IbgeCode, input.Name, state);
        await _cityRepository.SaveAsync(newCity);

        return CreateCityOutput.Success();        
    }
}
