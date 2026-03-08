using System.Security.Cryptography;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
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

    [HttpPost("sendOtp")]
    public async Task<IActionResult> SendOtp([FromBody] EmailRequest request)
    {
        var existingUser = await _service.GetUser(request.Email);

        if(!existingUser.Success)
        {
            return BadRequest(new {message = existingUser.Message});
        }

        Console.WriteLine("Entering to create a otp"+ request.Email);
        var random = RandomNumberGenerator.Create();
        var bytes = new byte[6 / 2]; 
        random.GetBytes(bytes);
        string otp = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 6);
        var result = await _service.sendOtpToUser(request.Email,otp);
        if (result.Success)
        {
            return Ok(new {message= result.Message, otp = otp, email = request.Email});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] EmailRequest request)
    {
        Console.WriteLine("Entered to change password for user",request.Email);
        var result = await _service.changePasswordForUser(request);
        if(result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [HttpPost("getProfile")]
    public async Task<IActionResult> GetProfile([FromBody] LoginModel request)
    {
        // var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Entered to get user profile "+request.Id);
        var result = await _service.GetDate(request.Id);
        if(result.Success)
        {
            return Ok(new {message = result.Message, data = result.User});
        }
        else
        {
            return BadRequest(new {message = result.Message});
        }
    }
}