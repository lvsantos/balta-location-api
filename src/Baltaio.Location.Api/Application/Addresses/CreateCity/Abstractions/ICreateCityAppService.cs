namespace Baltaio.Location.Api.Application.Addresses.CreateCity.Abstractions;

public interface ICreateCityAppService
{
    Task<CreateCityOutput> ExecuteAsync(CreateCityInput input, CancellationToken cancellationToken = default);
}
