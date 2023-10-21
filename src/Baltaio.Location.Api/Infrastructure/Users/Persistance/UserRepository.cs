using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Infrastructure.Users.Persistance;

public class UserRepository : IUserRepository
{
    private static Dictionary<Guid, User> _users = new();

    public Task<bool> ExistsAsync(string email)
    {
        return Task.FromResult(_users.Values.Any(u => u.Email == email));
    }

    public Task<User?> LoginAsync(User userToSearch)
    {
        User? user = _users
            .Values
            .FirstOrDefault(u => u.Email == userToSearch.Email && u.Password == userToSearch.Password);
        return Task.FromResult(user);
    }
    public Task SaveAsync(User user)
    {
        _users.Add(user.Id, user);
        return Task.CompletedTask;
    }
}
