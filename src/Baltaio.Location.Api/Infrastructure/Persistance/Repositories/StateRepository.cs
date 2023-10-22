using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories
{
    public class StateRepository : IStateRepository
    {
        public Task AddAllAsync(IEnumerable<State> states)
        {
            throw new NotImplementedException();
        }

        public Task<List<State>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<State?> GetAsync(string stateAbbreviation)
        {
            throw new NotImplementedException();
        }
    }
}
