using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using OrderService.Repositories;

namespace CartService.Services
{
    public class CartServices
    {
        private readonly MongoRepo _repo;
        private readonly IDistributedCache _cache;
        public CartServices(MongoRepo repo, IDistributedCache cache)
        {
            this._repo = repo;
            this._cache = cache;
        }

        private string GetCartKey(string userId)
        {
            return $"cart:{userId}";
        }

        public async Task<string> AddToCart(CartModel carts, string userId)
        {
            Console.WriteLine("userid in cart-> "+userId);
            Console.WriteLine("carts-> "+carts);

            var cart = await GetCarts(userId);

            var existingItem = cart.FirstOrDefault(x => x.Id == carts.Id);

            // if (existingItem != null)
            //     existingItem.BookPrice += carts.BookPrice;
            // else
                cart.Add(carts);

            await SaveCart(userId, cart);

            // await _repo.InsertMany("Carts", carts, userId);
            return "inserted";
        }

        private async Task SaveCart(string userId, List<CartModel> cart)
        {
            await _cache.SetStringAsync(
                GetCartKey(userId),
                JsonSerializer.Serialize(cart),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                });
        }
        public async Task<List<CartModel>> GetCarts(string userid)
        {
            var data = await _cache.GetStringAsync(GetCartKey(userid));

            if (data == null)
                return new List<CartModel>();

            return JsonSerializer.Deserialize<List<CartModel>>(data);
            // var filter = Builders<CartRequestModel>.Filter.Eq(c=> c.UserId, userid);

            // List<CartRequestModel> cartDocs = await _repo.GetCartAsync("Carts",filter);
            // List<CartModel> allBooks = cartDocs
            //     .Where(c => c.Carts != null)
            //     .SelectMany(c => c.Carts)
            //     .ToList();
            // return allBooks ?? new List<CartModel>();
        }

        public async Task RemoveCart(CartModel request)
        {
            var cart = await GetCarts(request.UserId);
            cart.RemoveAll(x => x.Id == request.Id);

            await SaveCart(request.UserId, cart);
            // var filter = Builders<CartRequestModel>.Filter.Eq(c=>c.UserId, request.UserId);

            // var update = Builders<CartRequestModel>.Update.PullFilter(c=>c.Carts,b=>b.Id == request.Id);
            
            // List<CartModel> remBooks = await _repo.UpdateCart("Carts",filter,update);
            // return remBooks;
        }
    }
}