using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Infrastructure.Addresses;

internal class CityRepository : ICityRepository
{
   
    private static Dictionary<int, City> _cities = new()
    {
        {5200050, new City()},
        {3100104, new City()},
        {5200100, new City()}
    };

    public Task<City?> GetAsync(int ibgeCode)
    {
        City? city = _cities.GetValueOrDefault(ibgeCode);        
        return Task.FromResult(city);
    }
    public Task<City?> SaveAsync(int ibgeCode, string NameCity, int stateCode)
    {
        City city = new()
        {
            IbgeCode = ibgeCode,
            NameCity = NameCity,
            StateCode = stateCode
        };
        _cities.Add(ibgeCode, city);
        return Task.FromResult(city);
    }
}
