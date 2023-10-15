namespace Baltaio.Location.Api.Domain.Locations;

public sealed class Address
{
    public Address(City city)
    {
        if (city is null)
            throw new ArgumentNullException(nameof(city));

        Code = Guid.NewGuid();
        City = city;
    }

    public Guid Code { get; init; }
    public City City { get; private set; }
}
