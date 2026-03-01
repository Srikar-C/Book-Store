
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration, AuthService AuthService)
    {
        this._configuration = configuration;
        this._authService = AuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel request)
    {
        string str = await _authService.RegisterUser(request);
        Console.WriteLine("message-> "+str);
        if(str!="Registration Success")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Register Successful"});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        LoginResponseModel str = await _authService.LoginUser(request);
        Console.WriteLine("message-> "+str);
        if(str.Message!="Login Successful")
        {
            return Unauthorized(new {message = str.Message});
            //return BadRequest(new {message = str});
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, str.Id),
            new Claim(ClaimTypes.Name, request.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds
        );

        return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token), id = str.Id});
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset([FromBody] ForgotModel request)
    {
        string str = await _authService.ResetPassword(request);
        Console.WriteLine("message-> "+str);
        if(str!="Password Resetted")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Password Resetted"});
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        return Ok(new {message = "Logged Out"});
    }

    [Authorize]
    [HttpPost("profile")]
    public async Task<IActionResult> Profile()
    {
        Console.WriteLine("id--> "+ClaimTypes.NameIdentifier);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine("userid-> "+userId);

        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("null-> "+userId);
            return Unauthorized();
        }

        UserModel str = await _authService.GetProfile(userId);

        Console.WriteLine("got user-> "+str);
        return Ok(new {message = "Got Dtls", user = str});

    }
}