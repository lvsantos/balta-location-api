using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateCity;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class CreateCityAppServiceTests
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _staterepository;
    private readonly CreateCityInput _input;
    private readonly City _city;
    private readonly State _state;

    public CreateCityAppServiceTests()
    {
        _cityRepository = Substitute.For<ICityRepository>();
        _staterepository = Substitute.For<IStateRepository>();
        _input = new(2900207, "Belo Horizonte", 99);
        _state = new(_input.StateCode, "MG", "Minas Gerais");
        _city = new(_input.IbgeCode, _input.Name, _state);
    }

    [Theory(DisplayName = "Deve retornar erros de validação se os dados de entrada forem inválidos")]
    [InlineData(0, "", 0)]
    [InlineData(0, null, 0)]
    public async Task Should_ReturnValidationErrors_When_InputDataIsInvalid(int ibgeCode, string name, int stateCode)
    {
        //Arrange
        CreateCityInput input = new(ibgeCode, name, stateCode);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput output = await service.ExecuteAsync(input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(3);
        output.Errors.Should().Contain("Código do IBGE inválido.");
        output.Errors.Should().Contain("Nome da cidade inválido.");
        output.Errors.Should().Contain("Código do estado inválido.");
    }
    [Fact(DisplayName = "Deve retornar erros de validação se o código do IBGE já existir.")]
    public async Task Should_ReturnValidationErrors_When_IbgeCodeAlreadyExists()
    {
        //Arrange
        _cityRepository
            .GetAsync(_input.IbgeCode)
            .Returns(_city);
        _staterepository
            .GetAsync(_input.StateCode)
            .Returns(_state);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput output = await service.ExecuteAsync(_input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.Should().Contain("Cidade já existente.");
    }
    [Fact(DisplayName = "Deve retornar erros de validação se o código do estado não existir.")]
    public async Task Should_ReturnValidationErrors_When_StateCodeDoesNotExist()
    {
        //Arrange
        _cityRepository
            .GetAsync(_input.IbgeCode)
            .Returns((City?)null);
        _staterepository
            .GetAsync(_input.StateCode)
            .Returns((State?)null);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput output = await service.ExecuteAsync(_input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(1);
        output.Errors.Should().Contain("Estado não encontrado.");
    }
    [Fact(DisplayName = "Deve retornar erros de validação quando a cidade já existir e o estado não existir")]
    public async Task Should_ReturnValidationErrors_When_CityAlreadyExistsAndStateDoesNotExist()
    {
        //Arrange
        _cityRepository
            .GetAsync(_input.IbgeCode)
            .Returns(_city);
        _staterepository
            .GetAsync(_input.StateCode)
            .Returns((State?)null);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput output = await service.ExecuteAsync(_input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeFalse();
        output.Errors.Should().HaveCount(2);
        output.Errors.Should().Contain("Cidade já existente.");
        output.Errors.Should().Contain("Estado não encontrado.");
    }
    [Fact(DisplayName = "Deve restaurar cidade se ela já existir e tiver sido removida.")]
    public async Task Should_RestoreCity_When_CityAlreadyExistsAndWasRemoved()
    {
        //Arrange
        _city.Remove();
        _cityRepository
            .GetAsync(_input.IbgeCode)
            .Returns(_city);
        _staterepository
            .GetAsync(_input.StateCode)
            .Returns(_state);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput output = await service.ExecuteAsync(_input);

        //Assert
        output.Should().NotBeNull();
        output.IsValid.Should().BeTrue();
        output.Errors.Should().BeEmpty();
        output.Id.Should().Be(_city.Code);
        _city.IsRemoved.Should().BeFalse();
        await _cityRepository
            .Received(1)
            .UpdateAsync(Arg.Any<City>());
    }
    [Fact(DisplayName = "Deve salvar endereço no banco de dados se código do IBGE existir.")]
    public async Task Should_SaveAddress_When_IbgeCodeExists()
    {
        //Arrange
        _cityRepository
            .GetAsync(_input.IbgeCode)
            .Returns((City?)null);
        _staterepository
            .GetAsync(_input.StateCode)
            .Returns(_state);
        CreateCityAppService service = new(_cityRepository, _staterepository);

        //Act
        CreateCityOutput result = await service.ExecuteAsync(_input);

        //Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Id.Should().Be(_city.Code);
        await _cityRepository
            .Received(1)
            .SaveAsync(Arg.Any<City>());
    }
}
