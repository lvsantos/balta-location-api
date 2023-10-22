using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Abstractions;

public interface IUserRepository
{
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> LoginAsync(User userToSearch, CancellationToken cancellationToken = default);
    Task SaveAsync(User user, CancellationToken cancellationToken = default);
}
