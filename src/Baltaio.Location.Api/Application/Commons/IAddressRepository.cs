using Baltaio.Location.Api.Domain.Locations;

namespace Baltaio.Location.Api.Application.Commons;

public interface IAddressRepository
{
    public Task SaveAsync(Address address);
}
