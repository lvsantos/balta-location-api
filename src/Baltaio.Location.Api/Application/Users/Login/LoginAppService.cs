using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Login;

public class LoginAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginAppService(IUserRepository userRepository, IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginOutput> ExecuteAsync(LoginInput input)
    {
        bool isValid = input.IsValid;
        if(!isValid)
        {
            return LoginOutput.ValidationErrors(input.Notifications);
        }

        User userToSearch = new(input.Email, input.Password);
        User? user = await _userRepository.LoginAsync(userToSearch);
        if(user is null)
        {
            return LoginOutput.ValidationErrors(new[] { "Usuário não encontrado." });
        }

        Token token = _jwtGenerator.GenerateToken(user);
        
        return LoginOutput.Success(token);
    }
}
