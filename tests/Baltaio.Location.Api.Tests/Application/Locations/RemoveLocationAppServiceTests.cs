using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.RemoveCity;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class RemoveLocationAppServiceTests
{
    [Fact(DisplayName = "Deve retornar erro de validação quando código do ibge não existir")]
    public async Task Should_ReturnValidationError_When_IbgeCodeDoesNotExist()
    {
        // Arrange
        var ibgeCode = 123456;
        RemoveCityInput input = new(ibgeCode);
        var cityRepository = Substitute.For<ICityRepository>();
        cityRepository.GetAsync(ibgeCode).Returns((City?)null);
        RemoveCityAppService removeCityAppService = new(cityRepository);

        // Act
        RemoveCityOutput output = await removeCityAppService.Execute(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.First().Should().Be("Código do IBGE não encontrado.");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando remover uma cidade já removida")]
    public async Task Should_ReturnValidationError_When_RemoveCityAlreadyRemoved()
    {
        // Arrange
        var ibgeCode = 123456;
        RemoveCityInput input = new(ibgeCode);
        var city = new City(ibgeCode, "Teste", new State(1, "Teste", "TS"));
        city.Remove();
        var cityRepository = Substitute.For<ICityRepository>();
        cityRepository.GetAsync(ibgeCode).Returns(city);
        RemoveCityAppService removeCityAppService = new(cityRepository);

        // Act
        RemoveCityOutput output = await removeCityAppService.Execute(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.First().Should().Be("Código do IBGE não encontrado.");
    }
    [Fact(DisplayName = "Deve remover a cidade quando código do ibge existir")]
    public async Task Should_RemoveCity_When_IbgeCodeExists()
    {
        // Arrange
        var ibgeCode = 123456;
        RemoveCityInput input = new(ibgeCode);
        var city = new City(ibgeCode, "Teste", new State(1, "Teste", "TS"));
        var cityRepository = Substitute.For<ICityRepository>();
        cityRepository.GetAsync(ibgeCode).Returns(city);
        RemoveCityAppService removeCityAppService = new(cityRepository);

        // Act
        RemoveCityOutput output = await removeCityAppService.Execute(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeTrue();
        output.Errors.Should().BeEmpty();
        await cityRepository.Received(1).UpdateAsync(city);
    }
}
