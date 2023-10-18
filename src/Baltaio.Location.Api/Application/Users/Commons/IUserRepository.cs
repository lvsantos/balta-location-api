using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Commons;

public interface IUserRepository
{
    Task<bool> ExistsAsync(string email);
    Task SaveAsync(User user);
}
