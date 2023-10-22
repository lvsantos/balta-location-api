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

    public async Task AddAllAsync(List<City> cities)
    {
        await _context.AddRangeAsync(cities);
        await _context.SaveChangesAsync();
    }

    public async Task<City?> GetAsync(int ibgeCode, CancellationToken cancellationToken = default)
    {
        City? city = await _context.Cities.FindAsync(ibgeCode, cancellationToken);
        return city;
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

    public async Task SaveAsync(City city, CancellationToken cancellationToken = default)
    {
        await _context.Cities.AddAsync(city, cancellationToken);
        await _context.SaveChangesAsync();
    }
}
