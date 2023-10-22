using Baltaio.Location.Api.Application.Addresses.Commons;
﻿using Baltaio.Location.Api.Application.Abstractions;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.UpdateCity;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class UpdateCityAppServiceTests
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _stateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCityAppServiceTests()
    {
        _cityRepository = Substitute.For<ICityRepository>();
        _stateRepository = Substitute.For<IStateRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    [Theory(DisplayName = "Deve retornar erros de validação quando a entrada dos dados for inválida")]
    [Trait("Application", "Locations")]
    [InlineData(0, "", 0)]
    [InlineData(0, null, 0)]
    public async Task Should_ReturnValidationErrors_When_InputIsInvalidAsync(int ibgeCode, string name, int stateCode)
    {
        // Arrange
        UpdateCityInput input = new (ibgeCode, name, stateCode);
        UpdateCityAppService service = new(_cityRepository, _stateRepository, _unitOfWork);

        // Act
        UpdateCityOutput output = await service.ExecuteAsync(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(3);
        output.Errors.Should().Contain(x => x == "O código do IBGE é obrigatório.");
        output.Errors.Should().Contain(x => x == "O nome da cidade é obrigatório.");
        output.Errors.Should().Contain(x => x == "O código do estado é obrigatório.");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando a cidade e o estado não existirem")]
    [Trait("Application", "Locations")]
    public async Task Should_ReturnValidationErrors_When_CityAndStateDoesNotExistAsync()
    {
        // Arrange
        UpdateCityInput input = new (1, "Contagem", 31);
        _cityRepository.GetAsync(input.IbgeCode).Returns((City?)null);
        _stateRepository.GetAsync(input.StateCode).Returns((State?)null);
        UpdateCityAppService service = new(_cityRepository, _stateRepository, _unitOfWork);

        // Act
        UpdateCityOutput output = await service.ExecuteAsync(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.Should().Contain(x => x == "A cidade não existe.");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando o estado não existir")]
    [Trait("Application", "Locations")]
    public async Task Should_ReturnValidationErrors_When_StateDoesNotExistAsync()
    {
        // Arrange
        UpdateCityInput input = new (1, "Contagem", 31);
        _cityRepository.GetAsync(input.IbgeCode).Returns(new City(1, "Contagem", new State(31, "Minas Gerais", "MG")));
        _stateRepository.GetAsync(input.StateCode).Returns((State?)null);
        UpdateCityAppService service = new(_cityRepository, _stateRepository);
        output.Errors.Should().HaveCount(2);
        output.Errors.Should().Contain(x => x == "A cidade não existe.");
        output.Errors.Should().Contain(x => x == "O estado não existe.");
    }
    [Fact(DisplayName = "Deve retornar erro de validação quando a cidade atualizada tiver sido removida logicamente")]
    [Trait("Application", "Locations")]
    public async Task Should_ReturnValidationErrors_When_CityIsDeletedAsync()
    {
        // Arrange
        UpdateCityInput input = new (1, "Contagem", 31);
        State state = new State(31, "Minas Gerais", "MG");
        City city = new City(1, "Contagem", state);
        city.Remove();
        _cityRepository.GetAsync(input.IbgeCode)
            .Returns(city);
        _stateRepository
            .GetAsync(input.StateCode)
            .Returns(state);
        UpdateCityAppService service = new(_cityRepository, _stateRepository, _unitOfWork);

        // Act
        UpdateCityOutput output = await service.ExecuteAsync(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.Should().Contain(x => x == "A cidade não existe.");
    }
    [Fact(DisplayName = "Deve retornar válido quando os dados de atualização forem válidos")]
    [Trait("Application", "Locations")]
    public async Task Should_ReturnValid_When_InputIsValidAsync()
    {
        // Arrange
        UpdateCityInput input = new (1, "Contagem", 31);
        _cityRepository.GetAsync(input.IbgeCode).Returns(new City(1, "Contagem", new State(31, "Minas Gerais", "MG")));
        _stateRepository.GetAsync(input.StateCode).Returns(new State(31, "Minas Gerais", "MG"));
        UpdateCityAppService service = new(_cityRepository, _stateRepository, _unitOfWork);

        // Act
        UpdateCityOutput output = await service.ExecuteAsync(input);

        // Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeTrue();
        output.Errors.Should().BeEmpty();
        _cityRepository.Received(1)
            .Update(Arg.Any<City>());
    }
}
