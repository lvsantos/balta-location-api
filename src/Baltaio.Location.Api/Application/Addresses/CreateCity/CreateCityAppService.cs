using Baltaio.Location.Api.Application.Abstractions;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateCity.Abstractions;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.CreateCity;

public class CreateCityAppService : ICreateCityAppService
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _stateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityAppService(
        ICityRepository cityRepository, 
        IStateRepository stateRepository,
        IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _stateRepository = stateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCityOutput> ExecuteAsync(
        CreateCityInput input,
        CancellationToken cancellationToken = default)
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
            _cityRepository.Update(city);
        }
        else
        {
            city = new(input.IbgeCode, input.Name, state!);
            await _cityRepository.SaveAsync(city);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return CreateCityOutput.Success(city);
    }
}
