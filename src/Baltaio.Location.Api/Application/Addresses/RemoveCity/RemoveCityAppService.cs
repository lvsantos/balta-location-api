using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.RemoveCity.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.RemoveCity;

internal class RemoveCityAppService : IRemoveCityAppService
{
    private readonly ICityRepository _cityRepository;

    public RemoveCityAppService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<RemoveCityOutput> ExecuteAsync(RemoveCityInput input)
    {
        City? city = await _cityRepository.GetAsync(input.IbgeCode);
        if (city is null || city.IsRemoved)
        {
            return RemoveCityOutput.ValidationError(new[] { "Código do IBGE não encontrado." });
        }

        city.Remove();
        await _cityRepository.UpdateAsync(city);

        return RemoveCityOutput.Success();
    }
}
