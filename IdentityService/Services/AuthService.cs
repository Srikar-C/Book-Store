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

        public async Task<string> RegisterUser(User user)
        {
            var userFilter = Builders<User>.Filter.Eq(u=> u.Username, user.Username);
            var passwordFilter = Builders<User>.Filter.Eq(u=> u.Password, user.Password);
           
            var userexist = Builders<User>.Filter.And(userFilter,passwordFilter);

            var fullMatch = await _repo.GetAsync("Users",userexist);

            if(fullMatch.Count>0)
            {
                return "1 User already exist. Please login";
            }
            
            var userMatch = await _repo.GetAsync("Users",userFilter);

            if(userMatch.Count>0)
            {
                return "2 User already exist with this username";
            }

            await _repo.InsertAsync("Users",user);
            return "3 Registration Success";
        }

        public async Task<string> LoginUser(User user)
        {
            Console.WriteLine("user details-> :"+user.Username+" "+user.Password);
            var userFilter = Builders<User>.Filter.Eq(u=> u.Username, user.Username);
            var passwordFilter = Builders<User>.Filter.Eq(u=> u.Password, user.Password);
           
            var userexist = Builders<User>.Filter.And(userFilter,passwordFilter);
            
            var fullMatch = await _repo.GetAsync("Users",userexist);

            if(fullMatch.Count>0)
            {
                return "1 Login Successful";
            }
            
            var userMatch = await _repo.GetAsync("Users",userFilter);

            if(userMatch.Count>0)
            {
                return "2 Password Incorrect";
            }

            return "3 User not exist please register";
        }
    }
}