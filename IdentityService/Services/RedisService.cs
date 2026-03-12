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

        public async Task<ResponseModel> GetOTPFromRedis(string email)
        {
            string key = $"otp:{email}";
            var data = await _redis.StringGetAsync(key);
            Console.WriteLine("otp-> :"+data);
            if (data.IsNullOrEmpty)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "OTP not present in Redis/ Expired"
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = data
            };
        }

        public async Task<ResponseModel> GetFromRedis(string email)
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
                Message = "Email retrieved from Redis",
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

        public async Task<ResponseModel> StoreOTPInRedis(string email, string otp)
        {
            Console.WriteLine("Storing otp in redis",email);
            string key = $"otp:{email}";

            await _redis.StringSetAsync(key,otp, TimeSpan.FromMinutes(2));

            return new ResponseModel
            {
                Success = true,
                Message = "OTP stored in Redis"
            };
        }

        public async Task<ResponseModel> DeleteOTPFromRedis(string email)
        {
            string key = $"otp:{email}";

            await _redis.KeyDeleteAsync(key);

            return new ResponseModel
            {
                Success = true,
                Message = "OTP cleared"
            };
        }
    }
}