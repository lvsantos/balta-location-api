using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.Commons
{
    public interface IStateRepository
    {
        Task AddAllAsync(IEnumerable<State> states);

        Task<State?> GetAsync(string stateAbbreviation);

        Task<List<State>?> GetAllAsync();
    }
}
