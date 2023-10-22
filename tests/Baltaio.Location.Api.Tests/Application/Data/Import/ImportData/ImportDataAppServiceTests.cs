using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Data.Import.Commons;
using Baltaio.Location.Api.Application.Data.Import.ImportData;
using Baltaio.Location.Api.Domain;
using FluentAssertions;
using NSubstitute;

namespace Baltaio.Location.Api.Tests.Application.Data.Import.ImportData
{
    public class ImportDataAppServiceTests
    {
        private ImportDataAppService _service;

        private readonly IFileRepository _fileRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        public ImportDataAppServiceTests()
        {
            _fileRepository = Substitute.For<IFileRepository>();
            _stateRepository = Substitute.For<IStateRepository>();
            _cityRepository = Substitute.For<ICityRepository>();

            _service = new(_fileRepository, _stateRepository, _cityRepository);
        }


        [Fact(DisplayName = "Deve retornar quantidades zeradas de dados importados quando o arquivo de entrada for inválido.")]
        [Trait("Application", "Data")]
        public async Task Should_ReturnZeroImportedData_When_InvalidFile()
        {
            // Arrange
            var file = new MemoryStream();

            _fileRepository.GetStates(file).Returns(new List<State>());
            _fileRepository.GetCities(file).Returns(new List<City>());

            // Act
            var importDataOutput = await _service.Execute(file);

            // Assert
            importDataOutput.Should().NotBeNull();
            importDataOutput.ImportedStates.Should().Be(0);
            importDataOutput.ImportedCities.Should().Be(0);
        }

        [Fact(DisplayName = "Deve retornar quantidades não-nulas de dados importados quando o arquivo de entrada for válido.")]
        [Trait("Application", "Data")]
        public async Task Should_ReturnNotZeroImportedData_When_ValidFile()
        {
            // Arrange
            var file = new MemoryStream();

            State state = new State(31, "Minas Gerais", "MG");
            _fileRepository.GetStates(file).Returns(new List<State>() { state });
            _fileRepository.GetCities(file).Returns(new List<City>() { new City(123, "Contagem", state)});

            // Act
            var importDataOutput = await _service.Execute(file);

            // Assert
            importDataOutput.Should().NotBeNull();
            importDataOutput.ImportedStates.Should().NotBe(0);
            importDataOutput.ImportedCities.Should().NotBe(0);
        }

    }
}
