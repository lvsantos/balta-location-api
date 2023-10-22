using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

public class StateRepository : IStateRepository
{
    private readonly ApplicationDbContext _context;

    public StateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAllAsync(IEnumerable<State> states)
    {
        throw new NotImplementedException();
    }

    public Task<List<State>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<State?> GetAsync(int stateCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
