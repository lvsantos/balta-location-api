using Baltaio.Location.Api.Domain.Locations;

namespace Baltaio.Location.Api.Application.Commons;

public interface ICityRepository
{
    public Task<City?> GetAsync(int ibgeCode);
}
