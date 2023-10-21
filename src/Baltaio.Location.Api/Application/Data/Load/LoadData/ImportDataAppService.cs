using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Data.Load.Commons;
using Baltaio.Location.Api.Infrastructure;

namespace Baltaio.Location.Api.Application.Data.Load.LoadData
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
            var cities = _fileRepository.GetCities(source);

            try
            {
                await _stateRepository.AddAllAsync(states);
                await _cityRepository.AddAllAsync(cities);
            }
            catch
            {

            }

            return new ImportDataOutput(states.Count, cities.Count);
        }

    }
}
