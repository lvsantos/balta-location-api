using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

internal class CityRepository : ICityRepository
{
    private static Dictionary<int, City> _cities = new()
    {
        {5200050, new City(5200050, "Contagem", "MG")},
        {3100104, new City(3100104, "BH", "MG")},
        {5200100, new City(5200100, "Sete Lagoas", "MG")}
    };

    public Task AddAllAsync(List<City> cities)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(City city)
    {
        throw new NotImplementedException();
    }

    public Task<City?> GetAsync(int ibgeCode)
    {
        City? city = _cities.GetValueOrDefault(ibgeCode);
        return Task.FromResult(city);
    }

    public Task<City?> GetAsync(string cityName)
    {
        throw new NotImplementedException();
    }

    public Task<City?> GetByStateAsync(string stateAbbreviation)
    {
        throw new NotImplementedException();
    }

    public Task<City?> SaveAsync(int IbgeCode, string NameCity, int StateCode)
    {
        throw new NotImplementedException();
    }
}
