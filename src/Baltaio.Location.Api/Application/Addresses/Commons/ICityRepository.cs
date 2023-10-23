using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.Commons;

public interface ICityRepository
{
    public Task AddAllAsync(List<City> cities);
    Task SaveAsync(City city, CancellationToken cancellationToken = default);
    Task<City?> GetAsync(int ibgeCode, CancellationToken cancellationToken = default);
    Task<List<City>> GetByStateOrCityAsync(string cityName, string stateName);
    void Update(City city);
    Task<City?> GetWithState(int cityCode);
}
