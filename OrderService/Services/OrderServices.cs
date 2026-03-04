using MongoDB.Driver;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderServices
    {
        private readonly MongoRepo _repo;

        public OrderServices(MongoRepo repo)
        {
            this._repo = repo;
        }

        public async Task<string> AddOrders(List<CartModel> orders, string userId)
        {
            await _repo.InsertOrdersMany("Orders",orders,userId);
            return "inserted orders";
        }

        public async Task<List<CartModel>> GetOrders(string userid)
        {
            var filter = Builders<OrderModel>.Filter.Eq(c=> c.UserId, userid);

            List<OrderModel> cartDocs = await _repo.GetOrderAsync("Orders",filter);
            List<CartModel> allBooks = cartDocs
                .Where(c => c.Orders != null)
                .SelectMany(c => c.Orders)
                .ToList();
            return allBooks ?? new List<CartModel>();
        }
    }
}