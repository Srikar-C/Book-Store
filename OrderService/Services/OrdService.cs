using MongoDB.Driver;
using OrderService.Repositories;
using StackExchange.Redis;

namespace OrderService.Services
{
    public class OrdService
    {
        private readonly MongoRepo _repo;
        private readonly IDatabase _redisDb;

        public OrdService(MongoRepo repo, IDatabase redisDb)
        {
            this._repo = repo;
            this._redisDb = redisDb;
        }

        public async Task<ResponseModel> PlaceOrder(string userId, List<BookModel> request)
        {
            var order = new OrderModel
            {
                UserId = userId,
                Orders = request
            };
            await _repo.InsertAsync("Orders", order);

            await _redisDb.KeyDeleteAsync($"cart:{userId}");

            return new ResponseModel
            {
                Success = true,
                Message = "Order placed successfully"
            };
        }

        public async Task<ResponseModel> GetOrders(string userId)
        {
            var filter = Builders<OrderModel>.Filter.Eq(o => o.UserId, userId);
            var orders = await _repo.FindOrderAsync<OrderModel>("Orders", filter);

            if (orders.Count == 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "No orders found for this user",
                    Data = new List<BookModel>()
                };
            }

            var allBooks = orders.SelectMany(o => o.Orders).ToList();

            return new ResponseModel
            {
                Success = true,
                Message = "Orders retrieved successfully",
                Data = allBooks
            };
            
        }
    }
}