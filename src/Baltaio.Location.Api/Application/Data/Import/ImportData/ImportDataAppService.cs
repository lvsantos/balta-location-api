using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Data.Import.Commons;
using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Baltaio.Location.Api.Application.Data.Import.ImportData
{
    public class ImportDataAppService : IImportDataAppService
    {
        private IFileRepository _fileRepository;
        private IStateRepository _stateRepository;
        private ICityRepository _cityRepository;

        public ImportDataAppService(IFileRepository fileRepository, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _fileRepository = fileRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        public async Task<ImportDataOutput> Execute(Stream source)
        {
            var states = _fileRepository.GetStates(source);
            await _stateRepository.AddAllAsync(states);

            var cities = _fileRepository.GetCities(source);
            await _cityRepository.AddAllAsync(cities);

            return new ImportDataOutput(states.Count, cities.Count);
        }

    }
}
