using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Application.Addresses.Commons;

public interface ICityRepository
{
    public Task<City?> GetAsync(int ibgeCode);
    public Task<City?> SaveAsync(int IbgeCode, string NameCity, int StateCode);

}
