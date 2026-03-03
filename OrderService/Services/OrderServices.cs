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

         public async Task<string> AddToCart(List<CartModel> carts, string userId)
        {
            Console.WriteLine("userid in cart-> "+userId);
            Console.WriteLine("carts-> "+carts);

            await _repo.InsertMany("Carts", carts, userId);
            return "inserted";
        }
        public async Task<List<CartModel>> GetCarts(string userid)
        {
            var filter = Builders<CartRequestModel>.Filter.Eq(c=> c.UserId, userid);

            List<CartRequestModel> cartDocs = await _repo.GetCartAsync("Carts",filter);
            List<CartModel> allBooks = cartDocs
                .Where(c => c.Carts != null)
                .SelectMany(c => c.Carts)
                .ToList();
            return allBooks ?? new List<CartModel>();
        }

        public async Task<List<CartModel>> RemoveCart(CartModel request)
        {
            var filter = Builders<CartRequestModel>.Filter.Eq(c=>c.UserId, request.UserId);

            var update = Builders<CartRequestModel>.Update.PullFilter(c=>c.Carts,b=>b.Id == request.Id);
            
            List<CartModel> remBooks = await _repo.UpdateCart("Carts",filter,update);
            return remBooks;
        }

        public async Task<string> AddOrders(List<CartModel> orders, string userId)
        {
            await _repo.InsertOrdersMany("Orders",orders,userId);
            return "inserted orders";
        }
    }
}