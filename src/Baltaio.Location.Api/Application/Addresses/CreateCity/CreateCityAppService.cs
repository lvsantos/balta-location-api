using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateCity.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.CreateCity;

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
        if(!input.IsValid)
            return CreateCityOutput.ValidationError(input.Notifications);

        City? city = await _cityRepository.GetAsync(input.IbgeCode);
        List<string> errors = new();
        if (city is not null && !city.IsRemoved)
        {
            errors.Add("Cidade já existente.");
        }
        State? state = await _stateRepository.GetAsync(input.StateCode);
        if (state is null)
        {
            errors.Add("Estado não encontrado.");
        }
        if (errors.Any())
        {
            return CreateCityOutput.ValidationError(errors);
        }

        if (city is not null)
        {
            city.Restore();
            city.Update(input.Name, state!);
            await _cityRepository.UpdateAsync(city);
        }
        else
        {
            city = new(input.IbgeCode, input.Name, state!);
            await _cityRepository.SaveAsync(city);
        }

        return CreateCityOutput.Success(city);
    }
}
