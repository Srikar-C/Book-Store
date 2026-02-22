using System.Reflection.Emit;
using IdentityService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginService _loginService;

    public AuthController(LoginService loginservice)
    {
        _loginService = loginservice;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _loginService.Login(request.User,request.Password);
        return Ok(new { message = "Login Successful" });
      //  Console.WriteLine(request.User+" "+request.Password);
    /*    if (request.User == "csk" && request.Password == "123456")
        {
            return Ok(new { message = "Login Successful" });
        }

        return Unauthorized(new { message = "Invalid credentials" }); */
    }
}

public class LoginRequest
{
    public string? User { get; set; }
    public string? Password { get; set; }
}