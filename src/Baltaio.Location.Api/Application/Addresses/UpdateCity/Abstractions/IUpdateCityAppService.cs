namespace Baltaio.Location.Api.Application.Addresses.UpdateCity.Abstractions;

public interface IUpdateCityAppService
{
    Task<UpdateCityOutput> ExecuteAsync(UpdateCityInput input, CancellationToken cancellationToken = default);
}
