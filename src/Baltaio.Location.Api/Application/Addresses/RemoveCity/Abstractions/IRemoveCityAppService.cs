namespace Baltaio.Location.Api.Application.Addresses.RemoveCity.Abstractions;

public interface IRemoveCityAppService
{
    Task<RemoveCityOutput> ExecuteAsync(RemoveCityInput input, CancellationToken cancellationToken = default);
}
