using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace IdentityService.Services
{
    public class AuthService
    {
        private readonly MongoRepo _repo;

        public AuthService(MongoRepo repo)
        {
            this._repo = repo;
        }

        public async Task<string> RegisterUser(RegisterModel user)
        {
            var userFilter = Builders<UserModel>.Filter.Eq(u=> u.Username, user.Username);
            var passwordFilter = Builders<UserModel>.Filter.Eq(u=> u.Password, user.Password);
           
            var userexist = Builders<UserModel>.Filter.And(userFilter,passwordFilter);

            var fullMatch = await _repo.GetAsync("Users",userexist);

            if(fullMatch.Count>0)
            {
                return "User already exist. Please login";
            }
            
            var userMatch = await _repo.GetAsync("Users",userFilter);

            if(userMatch.Count>0)
            {
                return "User already exist with this username";
            }

            if(!passwordConstraints(user.Password))
            {
                return "Password constraints missing";
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Password = hashPassword;

            UserModel newuser = new UserModel() {
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
            };

            await _repo.InsertAsync("Users",newuser);
            return "Registration Success";
        }

        public async Task<LoginResponseModel> LoginUser(LoginModel user)
        {
            Console.WriteLine("user details-> :",user.Username,user.Password);
            var userFilter = Builders<UserModel>.Filter.Eq(u=> u.Username, user.Username);

            var userMatch = await _repo.GetAsync("Users",userFilter);

            if(userMatch.Count == 0)
            {
                return new LoginResponseModel{Message = "User not found"};
            }
            
            var dbUser = userMatch.First();

            Console.WriteLine("dbuser-> ",dbUser.Email,dbUser.Username,dbUser.Password);

            bool validPassword = BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password);

            if(!validPassword)
            {
                return new LoginResponseModel{Message = "Password Incorrect"};
            }

            return new LoginResponseModel{Message = "Login Successful",Id = dbUser.Id};
        }

        public async Task<string> ResetPassword(ForgotModel user)
        {
            Console.WriteLine("user details-> :",user.Username,user.Password);

            if(user.Password != user.Cfn_Password)
            {
                return "Passwords are not same";
            }

            if(!passwordConstraints(user.Password))
            {
                return "Password should maintain constraints";
            }

            var userFilter = Builders<UserModel>.Filter.Eq(u=> u.Username, user.Username);
            var fullMatch = await _repo.GetAsync("Users",userFilter);

            if(fullMatch.Count == 0)
            {
                return "No User found";
            }
            
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var update = Builders<UserModel>.Update.Set(u=>u.Password,hashPassword);

            await _repo.ResetAsync("Users",userFilter,update);
            
            return "Password Resetted";
        }

        public bool passwordConstraints(string password)
        {
            var regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}";
            return Regex.IsMatch(password,regex);
        }

        public async Task<UserModel> GetProfile(string userid)
        {
            var filter = Builders<UserModel>.Filter.Eq(u=>u.Id,userid);
            var userDtls = await _repo.GetAsync("Users",filter);

            Console.WriteLine("userdtls--> ",userDtls);
            var user = userDtls.First();

            Console.WriteLine("user--> "+user);

            UserModel newuser = new UserModel() {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
            };
            return newuser;
        }

    }
}