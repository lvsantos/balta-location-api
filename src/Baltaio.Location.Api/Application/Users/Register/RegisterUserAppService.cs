using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Domain.Users;

namespace Baltaio.Location.Api.Application.Users.Register;

public class RegisterUserAppService
{
    private readonly IUserRepository _userRepository;

    public RegisterUserAppService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

        User user = new(input.Email, input.Password);
        await _userRepository.SaveAsync(user);

        return RegisterUserOutput.Success();
    }
}
