using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Data.Import.Commons
{
    public interface IFileRepository
    {
        List<State> GetStates(Stream source);
        List<City> GetCities(Stream source, List<State> states);

    }
}
