using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.Extensions.Options;

namespace Baltaio.Location.Api.Application.Users.Login;

internal class LoginAppService : ILoginAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly SaltSettings _saltSettings;

    public LoginAppService(
        IUserRepository userRepository,
        IJwtGenerator jwtGenerator,
        IOptions<SaltSettings> options)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _saltSettings = options.Value;
    }

    public async Task<LoginOutput> ExecuteAsync(LoginInput input)
    {
        bool isValid = input.IsValid;
        if(!isValid)
        {
            return LoginOutput.ValidationErrors(input.Notifications);
        }

        var userToSearch = User.Create(input.Email, input.Password, _saltSettings.Salt);
        User? user = await _userRepository.LoginAsync(userToSearch);
        if(user is null)
        {
            return LoginOutput.ValidationErrors(new[] { "Usuário não encontrado." });
        }

        Token token = _jwtGenerator.GenerateToken(user);
        
        return LoginOutput.Success(token);
    }
}
