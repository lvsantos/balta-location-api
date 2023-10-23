using Baltaio.Location.Api.Application.Abstractions;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Data.Import.Commons;
using Baltaio.Location.Api.Domain;

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
            states = await GetOnlyNotFilledInStates(states);
            await _stateRepository.AddAllAsync(states);

            var cities = _fileRepository.GetCities(source, states);
            cities = await GetOnlyNotFilledInCities(cities);
            await _cityRepository.AddAllAsync(cities);

            return new ImportDataOutput(states.Count, cities.Count);
        }

        private async Task<List<State>> GetOnlyNotFilledInStates(List<State> states)
        {
            var alreadyFilledInStates = await _stateRepository.GetAllAsync();

            return states.Where((state) =>
                        !alreadyFilledInStates.Exists(
                            (alreadyFilledInState) => alreadyFilledInState.Code == state.Code
                        )).ToList();
        }

        private async Task<List<City>> GetOnlyNotFilledInCities(List<City> cities)
        {
            var alreadyFilledInCities = await _cityRepository.GetByStateOrCityAsync("", "");

            return cities.Where((city) =>
                        !alreadyFilledInCities.Exists(
                            (alreadyFilledInCity) => alreadyFilledInCity.Code == city.Code
                        )).ToList();
        }

    }
}