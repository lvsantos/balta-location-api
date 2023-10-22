namespace Baltaio.Location.Api.Application.Users.Register.Abstraction;

public interface IRegisterUserAppService
{
    Task<RegisterUserOutput> ExecuteAsync(RegisterUserInput input, CancellationToken cancellationToken = default);
}
