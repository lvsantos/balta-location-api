using Baltaio.Location.Api.Domain.Addresses;

namespace Baltaio.Location.Api.Application.Addresses.Commons;

public interface IAddressRepository
{
    public Task SaveAsync(Address address);
}
