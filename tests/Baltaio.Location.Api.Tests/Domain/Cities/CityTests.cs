using Baltaio.Location.Api.Domain;
using FluentAssertions;

namespace Baltaio.Location.Api.Tests.Domain.Cities;

public class CityTests
{
    public class CityCreateTests
    {
        [Theory(DisplayName = "Deve lançar exceção ao criar cidade com nome inválido")]
        [InlineData(null, "Value cannot be null. (Parameter 'name')")]
        [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
        public void Should_ThrowException_When_NameIsInvalid(string name, string error)
        {
            // Arrange & Act
            Action act = () => new City(1, name, new State(1, "SP", "São Paulo"));

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage(error);
        }
        [Fact(DisplayName = "Deve lançar exceção ao criar cidade sem estado")]
        public void Should_ThrowException_When_StateIsNull()
        {
            // Arrange & Act
            Action act = () => new City(1, "São Paulo", null!);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Value cannot be null. (Parameter 'state')");
        }
        [Fact(DisplayName = "Deve lançar exceção ao criar cidade com código do ibge igual ou menos que zero")]
        public void Should_ThrowException_When_IbgeCodeIsLessOrEqualToZero()
        {
            // Arrange & Act
            Action act = () => new City(0, "São Paulo", new State(1, "SP", "São Paulo"));

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("O código do IBGE deve ser maior que zero. (Parameter 'code')");
        }
        [Fact(DisplayName = "Deve criar uma cidade com os dados válidos")]
        public void Should_CreateCity_When_DataIsValid()
        {
            // Arrange & Act
            City city = new(1, "São Paulo", new State(1, "SP", "São Paulo"));

            // Assert
            city.Should().NotBeNull();
            city.Code.Should().Be(1);
            city.Name.Should().Be("São Paulo");
            city.StateCode.Should().Be(1);
            city.State.Should().NotBeNull();
            city.State.Code.Should().Be(1);
            city.State.Abbreviation.Should().Be("SP");
            city.State.Name.Should().Be("São Paulo");
            city.IsRemoved.Should().BeFalse();
            city.RemovedAt.Should().BeNull();
        }
    }
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

    public class CityRemoveTests
    {
        [Fact(DisplayName = "Deve remover uma cidade")]
        public void Should_RemoveCity()
        {
            // Arrange
            City city = new(1, "São Paulo", new State(1, "SP", "São Paulo"));

            // Act
            city.Remove();

            // Assert
            city.IsRemoved.Should().BeTrue();
            city.RemovedAt.Should().NotBeNull();
            int oneSecond = 10_000_000;
            city.RemovedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(oneSecond));
        }
        [Fact(DisplayName = "Deve retornar exceção ao remover uma cidade já removida")]
        public void Should_ThrowException_When_CityIsAlreadyRemoved()
        {
            // Arrange
            City city = new(1, "São Paulo", new State(1, "SP", "São Paulo"));
            city.Remove();

            // Act
            Action act = () => city.Remove();

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("A cidade já foi removida.");
        }
        [Fact(DisplayName = "Deve restaurar cidade")]
        public void Should_RestoreCity()
        {
            // Arrange
            City city = new(1, "São Paulo", new State(1, "SP", "São Paulo"));
            city.Remove();

            // Act
            city.Restore();

            // Assert
            city.IsRemoved.Should().BeFalse();
            city.RemovedAt.Should().BeNull();
        }
    }
}
