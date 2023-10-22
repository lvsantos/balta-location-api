using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class GetCityAppServiceTests
{
    private readonly GetCityInput _input;
    private readonly ICityRepository _cityRepository;
    private readonly City _city;
    private readonly State _state;

    public GetCityAppServiceTests()
    {
        _input = new (1);
        _cityRepository = Substitute.For<ICityRepository>();
        _state = new(1, "Teste", "TS");
        _city = new(1, "Teste", _state);
    }

    [Fact(DisplayName = "Deve retornar erro de validação quando a cidade não existir")]
    public async Task Should_ReturnValidationError_When_CityDoesNotExist()
    {
        // Arrange
        _cityRepository
            .GetAsync(Arg.Any<int>())
            .Returns(Task.FromResult<City?>(null));
        GetCityAppService service = new (_cityRepository);

        // Act
        GetCityOutput result = await service.ExecuteAsync(_input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Contain("Cidade não encontrada");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando a cidade estiver removida logicamente")]
    public async Task Should_ReturnValidationError_When_CityIsSoftDeleted()
    {
        // Arrange
        GetCityInput input = new (1);
        _city.Remove();
        _cityRepository
            .GetWithState(Arg.Any<int>())
            .Returns(_city);
        GetCityAppService service = new (_cityRepository);

        // Act
        GetCityOutput result = await service.ExecuteAsync(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Contain("Cidade não encontrada");
    }
    [Fact(DisplayName = "Deve retornar a cidade quando a mesma existir")]
    public async Task Should_ReturnCity_When_CityExists()
    {
        // Arrange
        _cityRepository
            .GetWithState(Arg.Any<int>())
            .Returns(_city);
        GetCityAppService service = new (_cityRepository);

        // Act
        GetCityOutput result = await service.ExecuteAsync(_input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.IbgeCode.Should().Be(_city.Code);
        result.CityName.Should().Be(_city.Name);
        result.StateCode.Should().Be(_city.State.Code);
        result.StateAbreviation.Should().Be(_city.State.Abbreviation);
        result.StateName.Should().Be(_city.State.Name);
    }
}
