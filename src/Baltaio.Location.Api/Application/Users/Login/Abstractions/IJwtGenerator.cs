using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Login.Abstractions;

public interface IJwtGenerator
{
    Token GenerateToken(User user);
}
