using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Register.Abstraction;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.Extensions.Options;

namespace Baltaio.Location.Api.Application.Users.Register;

internal class RegisterUserAppService : IRegisterUserAppService
{
    private readonly IUserRepository _userRepository;
    private readonly SaltSettings _saltSettings;

    public RegisterUserAppService(IUserRepository userRepository, IOptions<SaltSettings> options)
    {
        _userRepository = userRepository;
        _saltSettings = options.Value;
    }

    public async Task<RegisterUserOutput> ExecuteAsync(RegisterUserInput input)
    {
        bool isValid = input.IsValid;
        if(!isValid)
        {
            return RegisterUserOutput.ValidationErrors(input.Notifications);
        }

        bool exists = await _userRepository.ExistsAsync(input.Email);
        if(exists)
        {
            return RegisterUserOutput.ValidationErrors(new[] { "O email informado já está em uso" });
        }

        var user = User.Create(input.Email, input.Password, _saltSettings.Salt);
        await _userRepository.SaveAsync(user);

        return RegisterUserOutput.Success();
    }
}
