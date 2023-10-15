using Baltaio.Location.Api.Application.Commons;
using Baltaio.Location.Api.Domain.Locations;

namespace Baltaio.Location.Api.Infrastructure.Addresses;

internal class AddressRepository : IAddressRepository
{
    private static Dictionary<Guid, Address> _addresses = new();

    public Task SaveAsync(Address address)
    {
        _addresses.Add(address.Code, address);
        return Task.CompletedTask;
    }
}
