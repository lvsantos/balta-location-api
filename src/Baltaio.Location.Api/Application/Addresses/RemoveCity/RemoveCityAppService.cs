using Baltaio.Location.Api.Application.Abstractions;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.RemoveCity.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.RemoveCity;

internal class RemoveCityAppService : IRemoveCityAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCityAppService(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoveCityOutput> ExecuteAsync(
        RemoveCityInput input,
        CancellationToken cancellationToken = default)
    {
        City? city = await _cityRepository.GetAsync(input.IbgeCode);
        if (city is null || city.IsRemoved)
        {
            return RemoveCityOutput.ValidationError(new[] { "Código do IBGE não encontrado." });
        }

        city.Remove();
        _cityRepository.Update(city);
        await _unitOfWork.CommitAsync(cancellationToken);

        return RemoveCityOutput.Success();
    }
}
