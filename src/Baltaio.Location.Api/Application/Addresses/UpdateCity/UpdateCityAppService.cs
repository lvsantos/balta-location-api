using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.UpdateCity.Abstractions;
using Baltaio.Location.Api.Domain;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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

        List<string> errors = new();
        City? city = await _cityRepository.GetAsync(input.IbgeCode, cancellationToken);
        if(city is null)
        {
            errors.Add("A cidade não existe.");
        }
        State? state = await _stateRepository.GetAsync(input.StateCode, cancellationToken);
        if(state is null)
        {
            errors.Add("O estado não existe.");
        }
        if(errors.Count > 0)
        {
            return UpdateCityOutput.ValidationErrors(errors);
        }

        city!.Update(input.Name, state!);
        await _cityRepository.UpdateAsync(city, cancellationToken);

        return UpdateCityOutput.Success();
    }
}
