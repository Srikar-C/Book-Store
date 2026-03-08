using System.Text.Json;
using StackExchange.Redis;

namespace IdentityService.Services
{
    public class RedisService
    {
        private readonly IDatabase _redis;

        public RedisService(IDatabase redis)
        {
            this._redis = redis;
        }

        internal async Task<ResponseModel> GetFromRedis(string email)
        {
            string key = $"user:{email}";
            var data = await _redis.StringGetAsync(key);
            if (data.IsNullOrEmpty)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Email found "
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "Emai retrieved from Redis",
            };
        }

        public async Task<ResponseModel> StoreRedisUserAsync(string email)
        {
            Console.WriteLine("Storing user in redis",email);
            string key = $"user:{email}";
            string json = JsonSerializer.Serialize(email);

            await _redis.StringSetAsync(key,json);

            return new ResponseModel
            {
                Success = true,
                Message = "User stored in Redis"
            };
        }

    }
}