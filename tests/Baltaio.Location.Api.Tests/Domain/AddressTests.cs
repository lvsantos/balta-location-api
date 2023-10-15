using Baltaio.Location.Api.Domain.Locations;
using FluentAssertions;

namespace Baltaio.Location.Api.Tests.Domain;

public class AddressTests
{
    [Fact(DisplayName = "Deve criar um endereço com sucesso")]
    public void Should_Create_Location_Successfully()
    {
        //Arrange
        City city = new();

        //Act
        Address address = new(city);

        //Assert
        address.Should().NotBeNull();
        address.City.Should().NotBeNull();
        address.Code.Should().NotBeEmpty();
    }
    [Fact(DisplayName = "Deve lançar uma exceção quando a cidade for nula")]
    public void Should_ThrowArgumentNullException_When_CityIsNull()
    {
        //Arrange
        City? city = null;

        //Act
        Action action = () => new Address(city);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName(nameof(city));
    }
}
