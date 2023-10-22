using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.UpdateCity.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.UpdateCity;

internal class UpdateCityAppService : IUpdateCityAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _stateRepository;

    public UpdateCityAppService(ICityRepository cityRepository, IStateRepository stateRepository)
    {
        _cityRepository = cityRepository;
        _stateRepository = stateRepository;
    }

    public async Task<UpdateCityOutput> ExecuteAsync(UpdateCityInput input, CancellationToken cancellationToken = default)
    {
        bool isValid = new UpdateCityInputValidation(input).IsValid;
        if(!isValid)
        {
            return UpdateCityOutput.ValidationErrors(input.Notifications);
        }

        City? city = await _cityRepository.GetAsync(input.IbgeCode, cancellationToken);
        if(city is null)
        {
            return UpdateCityOutput.ValidationErrors(new[] { "A cidade não existe." });
        }

        State? state = await _stateRepository.GetAsync(input.StateCode, cancellationToken);
        if(state is null)
        {
            return UpdateCityOutput.ValidationErrors(new[] { "O estado não existe." });
        }

        city.Update(input.Name, state);

        return UpdateCityOutput.Success();
    }
}
