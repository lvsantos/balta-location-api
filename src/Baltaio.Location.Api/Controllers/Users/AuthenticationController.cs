using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Application.Users.Register;
using Microsoft.AspNetCore.Mvc;

namespace Baltaio.Location.Api.Controllers.Users;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthenticationController(IUserRepository userRepository, IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
    {
        RegisterUserInput input = new(request.Email, request.Password);
        RegisterUserAppService service = new RegisterUserAppService(_userRepository);

        RegisterUserOutput output = await service.ExecuteAsync(input);

        if(!output.IsValid)
        {
            return BadRequest(output.Errors);
        }

        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserRequest request)
    {
        LoginInput input = new(request.Email, request.Password);
        LoginAppService service = new(_userRepository, _jwtGenerator);

        LoginOutput output = await service.ExecuteAsync(input);

        if(!output.IsValid)
        {
            return BadRequest(output.Errors);
        }

        return Ok(output.Token);
    }
}
