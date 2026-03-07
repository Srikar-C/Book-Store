using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using IdentityService.Repositories;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace IdentityService.Services
{
    public class AuthService
    {
        private readonly MongoRepo _repo;
        private readonly IConfiguration _config;

        public AuthService(MongoRepo repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }

        public async Task<ResponseModel> Register(RegisterModel request)
        {
            Console.WriteLine("Inside Register Service for: " + request.Username);
            var nameFilter = Builders<RegisterModel>.Filter.Eq(u => u.Username, request.Username);
            var emailFilter = Builders<RegisterModel>.Filter.Eq(u => u.Email, request.Email);

            var userFilter = Builders<RegisterModel>.Filter.And(nameFilter, emailFilter);

            var existingUser = await _repo.FindUserAsync("Users",userFilter);

            if(existingUser.Count > 0)
            {
                return new ResponseModel { Success = false, Message = "Username and Email already exist" };
            }

            var existingUserByName = await _repo.FindUserAsync("Users",nameFilter);
            if (existingUserByName.Count > 0)
            {
                return new ResponseModel { Success = false, Message = "Username already exists" };
            }

            var existingUserByEmail = await _repo.FindUserAsync("Users",emailFilter);
            if (existingUserByEmail.Count > 0)
            {
                return new ResponseModel { Success = false, Message = "Email already exists" };
            }

            if(!passwordConstraints(request.Password))
            {
                return new ResponseModel { Success = false, Message = "Password constraints not met" };
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            request.Password = hashPassword;

            await _repo.InsertUserAsync("Users",request);

            return new ResponseModel { Success = true, Message = "User registered successfully" };
        }

        public bool passwordConstraints(string password)
        {
            var regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}";
            return Regex.IsMatch(password,regex);
        }

        public async Task<ResponseModel> Login(LoginModel request)
        {
            Console.WriteLine("Inside Login Service for: " + request.Email);
            
            var emailFilter = Builders<RegisterModel>.Filter.Eq(u => u.Email, request.Email);
            var existingUser = await _repo.FindUserAsync("Users",emailFilter);

            if(existingUser.Count == 0)
            {
                
                var nameFilter = Builders<RegisterModel>.Filter.Eq(u => u.Username, request.Email);
                existingUser = await _repo.FindUserAsync("Users",nameFilter);
                if(existingUser.Count == 0){
                    return new ResponseModel { Success = false, Message = "User not found" };
                }
            }

            var user = existingUser[0];

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                return new ResponseModel { Success = false, Message = "Invalid password" };
            }

            UserModel loggedInUser = new UserModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };         

            var tokenString = GenerateJSONWebToken(loggedInUser);

            if(user.Username=="admin")
            {
                return new ResponseModel { Success = true, Message = "Admin login successful", Token = tokenString };
            }

            return new ResponseModel { Success = true, Message = "Login successful", Token = tokenString };
        }

        public string GenerateJSONWebToken(UserModel userInfo)
        {
            // Implement JWT generation logic here
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new Claim("id", userInfo.Id),
                new Claim("username", userInfo.Username)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponseModel> Logout()
        {
            // Implement logout logic here (e.g., invalidate token, clear session, etc.)
            // For stateless JWT, you might not need to do anything on the server side for logout.
            return new ResponseModel { Success = true, Message = "Logout successful" };
        }

        internal object GenerateAdminToken()
        {
            throw new NotImplementedException();
        }
    }
}