
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService AuthService)
    {
        this._authService = AuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User request)
    {
        string str = await _authService.RegisterUser(request);
        Console.WriteLine("message-> "+str);
        if(str!="3 Registration Success")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Register Successful"});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User request)
    {
        string str = await _authService.LoginUser(request);
        Console.WriteLine("message-> "+str);
        if(str!="1 Login Successful")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Login Successful"});
    }
}