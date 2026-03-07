using OrderService.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderService.Services
{
    public class CartService
    {
        private readonly MongoRepo _repo;
        private readonly IDatabase _redisDb;

        public CartService(MongoRepo repo, IDatabase redisDb)
        {
            this._repo = repo;
            this._redisDb = redisDb;
        }

        public async Task<ResponseModel> StoreCart(string userid,List<BookModel> request)
        {
            string key = $"cart:{userid}";

            string json = JsonSerializer.Serialize(request);

            await _redisDb.StringSetAsync(key, json); 

            return new ResponseModel
            {
                Success = true,
                Message = "Cart stored in Redis"
            };
        }

        internal async Task<ResponseModel> GetCart(string userId)
        {
            string key = $"cart:{userId}";

            var data = await _redisDb.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return new ResponseModel
                {
                    Success = true,
                    Message = "Cart is Empty"
                };

            List<BookModel> json = JsonSerializer.Deserialize<List<BookModel>>(data);
            return new ResponseModel
            {
                Success = true,
                Message = "Cart stored in Redis",
                Data = json
            };
        }

        public async Task<ResponseModel> RemoveFromCart(string userId, BookModel request)
        {
            string key = $"cart:{userId}";
            var data = await _redisDb.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cart is Empty"
                };

            var carts = JsonSerializer.Deserialize<List<BookModel>>(data);

            carts = carts.Where(c => c.Id != request.Id).ToList();

            await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(carts));
            return new ResponseModel
            {
                Success = true,
                Message = "Book removed from cart"
            };
        }
    }
}