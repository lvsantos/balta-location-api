namespace Baltaio.Location.Api.Application.Users.Login.Abstractions;

public interface ILoginAppService
{
    Task<LoginOutput> ExecuteAsync(LoginInput input);
}
