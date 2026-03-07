using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel request)
    {
        Console.WriteLine("Received registration request for: " + request.Username);
        ResponseModel result = await _service.Register(request);

        if (result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        Console.WriteLine("Received login request for: " + request.Email);

        ResponseModel result = await _service.Login(request);

        if (result.Success)
        {
            return Ok(new {message= result.Message, token= result.Token});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Console.WriteLine("Received logout request.");
        ResponseModel result = await _service.Logout();

        if (result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }
}