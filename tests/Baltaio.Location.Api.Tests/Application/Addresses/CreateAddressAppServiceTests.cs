using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Domain.Addresses;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Addresses;

public class CreateAddressAppServiceTests
{
    private readonly ICityRepository _cityRepositoryMock;
    private readonly IAddressRepository _addressRepositoryMock;

    public CreateAddressAppServiceTests()
    {
        _cityRepositoryMock = Substitute.For<ICityRepository>();
        _addressRepositoryMock = Substitute.For<IAddressRepository>();
    }

    [Fact(DisplayName = "Deve retornar mensagem de erro se código do IBGE não existir.")]
    public async Task Should_ReturnErrorMessage_When_IbgeCodeDoesNotExist()
    {
        //Arrange
        int ibgeCode = 42001010;
        CreateAddressInput input = new(ibgeCode);
        _cityRepositoryMock
            .GetAsync(ibgeCode)
            .Returns((City?)null);
        CreateAddressAppService service = new(_cityRepositoryMock, _addressRepositoryMock);

        //Act
        CreateAddressOutput result = await service.ExecuteAsync(input);

        //Assert
        result.Message.Should().Be("Código IBGE não encontrado.");
        result.AddressCode.Should().BeEmpty();
    }
    [Fact(DisplayName = "Deve salvar endereço no banco de dados se código do IBGE existir.")]
    public async Task Should_SaveAddress_When_IbgeCodeExists()
    {
        //Arrange
        int ibgeCode = 2900207;
        CreateAddressInput input = new(ibgeCode);
        _cityRepositoryMock
            .GetAsync(ibgeCode)
            .Returns(new City());
        CreateAddressAppService service = new(_cityRepositoryMock, _addressRepositoryMock);

        //Act
        CreateAddressOutput result = await service.ExecuteAsync(input);

        //Assert
        result.Message.Should().Be("Endereço criado com sucesso.");
        result.AddressCode.Should().NotBeEmpty();
        await _addressRepositoryMock.Received(1).SaveAsync(Arg.Any<Address>());
    }
}
