using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Application.Users.Register;
using Baltaio.Location.Api.Application.Users.Register.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Baltaio.Location.Api.Controllers.Users;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IRegisterUserAppService _registerUserAppService;
    private readonly ILoginAppService _loginAppService;

    public AuthenticationController(IRegisterUserAppService registerUserAppService, ILoginAppService loginAppService)
    {
        _registerUserAppService = registerUserAppService;
        _loginAppService = loginAppService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
    {
        RegisterUserInput input = new(request.Email, request.Password);

        RegisterUserOutput output = await _registerUserAppService.ExecuteAsync(input);

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

        LoginOutput output = await _loginAppService.ExecuteAsync(input);

        if(!output.IsValid)
        {
            return BadRequest(output.Errors);
        }

        return Ok(output.Token);
    }
}
