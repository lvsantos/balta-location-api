using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

internal class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private static Dictionary<int, City> _cities = new();

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
    public Task<City?> GetByStateOrCityAsync(string stateName, string cityName)
    {
        var searchCity = _context.Cities.Where(c => c.Name == cityName || c.State.Name == stateName).FirstOrDefault();
        return Task.FromResult(searchCity);
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
