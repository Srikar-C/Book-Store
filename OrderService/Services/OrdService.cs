using MongoDB.Driver;
using OrderService.Repositories;
using StackExchange.Redis;

namespace OrderService.Services
{
    public class OrdService
    {
        private readonly MongoRepo _repo;
        private readonly IDatabase _redisDb;
        private readonly KafkaProducer _producer;

        public OrdService(MongoRepo repo, IDatabase redisDb, KafkaProducer producer)
        {
            this._repo = repo;
            this._redisDb = redisDb;
            this._producer = producer;
        }

        public async Task<ResponseModel> PlaceOrder(string userId, List<BookModel> request)
        {
            Console.WriteLine("Placing order for user: " + userId);
            var order = new OrderModel
            {
                UserId = userId,
                Orders = request
            };
            await _repo.InsertAsync("Orders", order);

            await _redisDb.KeyDeleteAsync($"cart:{userId}");

            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<CartModel>.Update.Set(c => c.Carts, new List<BookModel>());
            await _repo.UpdateCartAsync("Carts", filter, update);
            Console.WriteLine("Cart cleared for user: " + userId);

            var orderEvent = new OrderCreatedEvent
            {
                UserId = userId,
                Books = request.Select(b => new BookModel
                {
                    Id = b.Id,
                    Quantity = b.Quantity,
                    SoldOut = b.SoldOut,
                    Count = b.Count
                }).ToList(),
                CreatedAt = DateTime.UtcNow
            };

            await _producer.ProduceAsync("order-created", orderEvent);

            Console.WriteLine("Kafka event published: order-created");

            return new ResponseModel
            {
                Success = true,
                Message = "Order placed successfully"
            };
        }

        public async Task<ResponseModel> GetOrders(string userId)
        {
            Console.WriteLine("Getting orders for user: " + userId);
            var filter = Builders<OrderModel>.Filter.Eq(o => o.UserId, userId);
            var orders = await _repo.FindOrderAsync("Orders", filter);

            if (orders.Count == 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "No orders found for this user",
                    Books = new List<BookModel>()
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "Orders retrieved successfully",
                Orders = orders 
            };
        }
    }
}