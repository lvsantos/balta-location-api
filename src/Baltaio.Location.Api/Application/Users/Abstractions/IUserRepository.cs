using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Abstractions;

public interface IUserRepository
{
    Task<bool> ExistsAsync(string email);
    Task<User?> LoginAsync(User user);
    Task SaveAsync(User user);
}
