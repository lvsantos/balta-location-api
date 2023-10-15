using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Application.Users.Register;
using Microsoft.AspNetCore.Mvc;

namespace Baltaio.Location.Api.Controllers.Users;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}
