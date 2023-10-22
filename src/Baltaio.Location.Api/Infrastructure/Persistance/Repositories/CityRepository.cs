using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

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
    public async Task<List<City>> GetByStateOrCityAsync(string cityName, string stateName)
    {
        IQueryable<City> query = _context.Cities.AsQueryable();

        if(!string.IsNullOrEmpty(cityName))
        {
            query = query.Where(c => c.Name == cityName);
        }
        if(!string.IsNullOrEmpty(stateName))
        {
            query = query.Where(c => c.State.Name == stateName);
        }

        return query.ToList();
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
    }

    public void Update(City city)
    {
        _context.Cities.Update(city);
    }
    public async Task<City?> GetWithState(int cityCode)
    {
        City? city = await _context.Cities
            .Include(c => c.State)
            .FirstOrDefaultAsync(c => c.Code == cityCode);
        return city;
    }

    public void Update(City city, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
