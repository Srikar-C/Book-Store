using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityService.Repositories;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using StackExchange.Redis;

namespace IdentityService.Services
{
    public class AuthService
    {
        private readonly MongoRepo _repo;
        private readonly IConfiguration _config;
        private readonly RedisService _redis;

        public AuthService(MongoRepo repo, IConfiguration config, RedisService redis)
        {
            this._repo = repo;
            this._config = config;
            this._redis = redis;
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

            var redisresponse = await _redis.SetUserInRedis(request);

            ResponseModel sendotp = await sendOtpToUser(request.Email);

           // await _repo.InsertUserAsync("Users",request);

            return new ResponseModel { Success = true, Message = "User registered successfully", User = new RegisterModel{Username = request.Username, Password = sendotp.User.Username} };
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
            return new ResponseModel { Success = true, Message = "Logout successful" };
        }

        public async Task<ResponseModel> sendOtpToUser(string email)
        {
            Console.WriteLine("Entering to create a otp"+ email);
            var random = RandomNumberGenerator.GetInt32(100000, 999999);
            string otp = random.ToString();
            try
            {       
                Console.WriteLine("Values to send OTP-> "+email+" "+otp);
                var fromAddress = new MailAddress("dnreply20@gmail.com", "Book Store");
                var toAddress = new MailAddress(email);
                const string subject = "Your OTP Code";
                string body = $"Hello,<br /><br />Following is your OTP: <b>{otp}</b>"; 

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", 
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, "ndrx ovtq fbdf bmls") 
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }

                await _redis.StoreOTPInRedis(email,otp);

                return new ResponseModel { Success = true, Message = "OTP Sent Successfully", User = new RegisterModel{ Username = otp }};
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> VerifyOTP(EmailRequest request)
        {
            Console.WriteLine("Get into service for OTP verify");
            var getOTPFromRedis = await _redis.GetOTPFromRedis(request.Email);
            Console.WriteLine("Details from Redis",getOTPFromRedis);
            if(getOTPFromRedis.Success)
            {
                Console.WriteLine("OTP-> "+request.Password+" "+getOTPFromRedis.Message+" -> "+(request.Password == getOTPFromRedis.Message)+" , "+(request.Password == getOTPFromRedis.Message));
                if(request.Password == getOTPFromRedis.Message)
                {
                    var delete = await _redis.DeleteOTPFromRedis(request.Email);
                    if(delete.Success)
                    {
                        return new ResponseModel
                        {
                            Success = true,
                            Message = "OTP verified"
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = delete.Message
                        };
                    }

                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid OTP"
                    };
                }
            }
            return new ResponseModel
            {
                Success = false,
                Message = "OTP Expired"
            };

        }

        public async Task<ResponseModel> GetUser(string email)
        {
            Console.WriteLine("Searching for user in redis",email);
            var getFromRedis = await _redis.GetEmailFromRedis(email);

            if(getFromRedis.Success)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "Email present"
                };
            }

            var getFromDBAndStoreinRedis = await GetUserFromDB(email);
            if(getFromDBAndStoreinRedis.Success)
            {
                Console.WriteLine("Present in DB, Storing in Redis");
                await _redis.StoreRedisUserAsync(email);
                return new ResponseModel
                {
                    Success = true,
                    Message = "Email present"
                };
            }
            return new ResponseModel
            {
                Success = false,
                Message = "Email not present"
            };
        }

        public async Task<ResponseModel> GetUserFromDB(string email)
        {
            Console.WriteLine("Getting user from DB",email);
            var filter = Builders<RegisterModel>.Filter.Eq(u=>u.Email,email);
            var result = await _repo.FindUserAsync("Users",filter);

            if(result.Count==0)
            {
                return new ResponseModel{ Success = false, Message = "Not an Existing User"};
            }

            return new ResponseModel {Success = true, Message = "Existing User"};
        }

        public async Task<ResponseModel> ChangePasswordForUser(EmailRequest request)
        {
            var filter = Builders<RegisterModel>.Filter.Eq(u=>u.Email,request.Email);
            if(!passwordConstraints(request.Password))
            {
                return new ResponseModel { Success = false, Message = "Password constraints not met" };
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashPassword;

            var update = Builders<RegisterModel>.Update.Set(c => c.Password, request.Password);
            await _repo.UpdatePasswordAsync("Users",filter,update);
            return new ResponseModel
            {
                Success = true,
                Message = "User Password Changed"
            };
        }

        public async Task<ResponseModel> GetDate(string userId)
        {
            Console.WriteLine("Entered userid-> ",userId);
            var filter = Builders<RegisterModel>.Filter.Eq(u=>u.Id,userId);
            List<RegisterModel> user = await _repo.FindUserAsync("Users", filter);
            if(user.Count<=0)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "user not found"
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "User details fetched",
                User = user[0]
            };
        }

        public async Task<ResponseModel> GetUserFromCache(string username)
        {
            Console.WriteLine("Entered",username);
            var user = await _redis.GetUserFromRedis(username);
            if(user.Success)
            {
                await _repo.InsertUserAsync("Users",user.User);
                
                await _redis.KeyUserDeleteAsync(username);

                return new ResponseModel
                {
                    Success = true,
                    Message = "Retrieved user from cache and storedin db and removed from cache",
                    User = user.User,
                };
            }
            Console.WriteLine("Entered",username);
            return new ResponseModel
            {
                Success = false,
                Message = "Error in retreiving from redis",
            };
        }
    }
}