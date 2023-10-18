using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private static Dictionary<Guid, User> _users = new();

    public Task<bool> ExistsAsync(string email)
    {
        return Task.FromResult(_users.Values.Any(u => u.Email == email));
    }

    public Task SaveAsync(User user)
    {
        _users.Add(user.Id, user);
        return Task.CompletedTask;
    }
}
