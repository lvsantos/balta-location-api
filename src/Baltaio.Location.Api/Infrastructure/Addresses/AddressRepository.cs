using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Infrastructure.Addresses;

[Obsolete("Não será usado mais")]
internal class AddressRepository : IAddressRepository
{
    private static Dictionary<Guid, Address> _addresses = new();

    public Task SaveAsync(Address address)
    {
        _addresses.Add(address.Code, address);
        return Task.CompletedTask;
    }
}
