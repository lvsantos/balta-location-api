using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.Commons;

public interface ICityRepository
{
    public Task AddAllAsync(List<City> cities);
    public Task<City?> GetByStateAsync(string stateAbbreviation);
    Task SaveAsync(City city, CancellationToken cancellationToken = default);
    Task<City?> GetAsync(int ibgeCode, CancellationToken cancellationToken = default);
    void Update(City city, CancellationToken cancellationToken = default);
    Task<List<City>> GetByStateOrCityAsync(string cityName, string stateName);
}
