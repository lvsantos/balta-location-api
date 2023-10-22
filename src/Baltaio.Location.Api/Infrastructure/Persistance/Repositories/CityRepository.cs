using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

internal class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAllAsync(List<City> cities)
    {
        throw new NotImplementedException();
    }

    public async Task<City?> GetAsync(int ibgeCode, CancellationToken cancellationToken = default)
    {
        City? city = await _context.Cities.FindAsync(ibgeCode, cancellationToken);
        return city;
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

    public void Update(City city, CancellationToken cancellationToken = default)
    {
        _context.Cities.Update(city);
    }
}
