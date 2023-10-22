using Baltaio.Location.Api.Application.Abstractions;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.RemoveCity;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class RemoveCityAppServiceTests
{
    private readonly RemoveCityInput _input;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICityRepository _cityRepository;

    public RemoveCityAppServiceTests()
    {
        _input = new(123456);
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cityRepository = Substitute.For<ICityRepository>();
    }

    [Fact(DisplayName = "Deve retornar erro de validação quando código do ibge não existir")]
    public async Task Should_ReturnValidationError_When_IbgeCodeDoesNotExist()
    {
        // Arrange
        _cityRepository.GetAsync(_input.IbgeCode)
            .Returns((City?)null);
        RemoveCityAppService removeCityAppService = new(_cityRepository, _unitOfWork);

        // Act
        RemoveCityOutput output = await removeCityAppService.ExecuteAsync(_input);

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
        var city = new City(_input.IbgeCode, "Teste", new State(1, "Teste", "TS"));
        city.Remove();
        _cityRepository.GetAsync(_input.IbgeCode).Returns(city);
        RemoveCityAppService removeCityAppService = new(_cityRepository, _unitOfWork);

        // Act
        RemoveCityOutput output = await removeCityAppService.ExecuteAsync(_input);

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
        var city = new City(_input.IbgeCode, "Teste", new State(1, "Teste", "TS"));
        _cityRepository.GetAsync(_input.IbgeCode).Returns(city);
        RemoveCityAppService removeCityAppService = new(_cityRepository, _unitOfWork);

        // Act
        RemoveCityOutput output = await removeCityAppService.ExecuteAsync(_input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeTrue();
        output.Errors.Should().BeEmpty();
        _cityRepository.Received(1).Update(city);
    }
}
