using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;
using System.Runtime.CompilerServices;

namespace Baltaio.Location.Api.Tests.Application.Locations;

public class CreateLocationAppServiceTests
{
    private readonly ICityRepository _cityRepositoryMock;
    private readonly IStateRepository _staterepository;

    public CreateLocationAppServiceTests()
    {
        _cityRepositoryMock = Substitute.For<ICityRepository>();
        _staterepository = Substitute.For<IStateRepository>();
    }

    [Fact(DisplayName = "Deve retornar mensagem de erro se código do IBGE não existir.")]
    public async Task Should_ReturnErrorMessage_When_IbgeCodeDoesNotExist()
    {
        //Arrange
        int ibgeCode = 42001010;
        string nameCity = "Belo Horizonte";
        int stateCode = 99;
        CreateCityInput input = new(ibgeCode, nameCity, stateCode);
        _cityRepositoryMock
            .GetAsync(ibgeCode)
            .Returns((City?)null);
        CreateCityAppService service = new(_cityRepositoryMock, _staterepository);

        //Act
        CreateCityOutput result = await service.ExecuteAsync(input);

        //Assert
        result.Errors.Should().Contain("Código IBGE não encontrado.");
        //result.AddressCode.Should().BeEmpty();
    }
    [Fact(DisplayName = "Deve salvar endereço no banco de dados se código do IBGE existir.")]
    public async Task Should_SaveAddress_When_IbgeCodeExists()
    {
        //Arrange
        int ibgeCode = 2900207;
        string nameCity = "Belo Horizonte";
        int stateCode = 99;
        CreateCityInput input = new(ibgeCode, nameCity, stateCode);
        _cityRepositoryMock
            .GetAsync(ibgeCode)
            .Returns(new City(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<State>()));
        CreateCityAppService service = new(_cityRepositoryMock, _staterepository);

        //Act
        CreateCityOutput result = await service.ExecuteAsync(input);

        //Assert
        result.Errors.Should().Contain("Endereço criado com sucesso.");
        //result.AddressCode.Should().NotBeEmpty();
        //await _addressRepositoryMock.Received(1).SaveAsync(Arg.Any<Address>());
    }
}
