using IdentityService.Repositories;
using MongoDB.Driver;

namespace IdentityService.Services
{

    public class LoginService
    {
        private readonly MongoRepo _repo;

        public LoginService(MongoRepo repo)
        {
            _repo = repo;
        }

        public async Task RegisterUser(User user)
        {
            await _repo.InsertAsync("Users", user);
        }

        public async Task<List<User>> GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);

            return await _repo.GetAsync("Users", filter);
        }
        public void Login(string user,string password)
        {
            Console.WriteLine(user+" "+password);

        }
    }
}