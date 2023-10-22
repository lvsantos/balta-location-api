using Baltaio.Location.Api.Domain;
using FluentAssertions;

namespace Baltaio.Location.Api.Tests.Domain.Cities;

public class CityTests
{
    public class CityUpdateTests
    {
        private readonly City _city;
        private readonly string _newCityName;
        private readonly State _newState;

        public CityUpdateTests()
        {
            _city = new City(1, "São Paulo", new State(1, "SP", "São Paulo"));
            _newCityName = "Rio de Janeiro";
            _newState = new State(2, "RJ", "Rio de Janeiro");
        }

        [Fact(DisplayName = "Deve atualizar uma cidade com os dados válidos")]
        public void Should_UpdateCity_When_DataIsValid()
        {
            // Arrange & 
            _city.Update(_newCityName, _newState);

            // Assert
            _city.Name.Should().Be(_newCityName);
            _city.State.Should().Be(_newState);
        }
        [Theory(DisplayName = "Deve retornar exceção ao atualizar cidade com nome inválido")]
        [InlineData(null)]
        [InlineData("")]
        public void Should_ThrowException_When_NameIsInvalid(string newName)
        {
            // Arrange & Act
            Action act = () => _city.Update(newName, _newState);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage($"O nome da cidade é obrigatório. (Parameter '{nameof(newName)}')");
        }
        [Fact(DisplayName = "Deve retornar exeção ao atualizar cidade sem estado")]
        public void Should_ThrowException_When_StateIsNull()
        {
            // Arrange & Act
            Action act = () => _city.Update(_newCityName, null!);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage($"Value cannot be null. (Parameter '{nameof(State)}')");
        }
    }
}
