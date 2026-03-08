using MongoDB.Driver;
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

        public async Task<ResponseModel> StoreRedisCartAsync(string userid,List<BookModel> request)
        {
            Console.WriteLine("Storing cart in Redis for user: " + userid);
            string key = $"cart:{userid}";

            string json = JsonSerializer.Serialize(request);

            await _redisDb.StringSetAsync(key, json); 

            return new ResponseModel
            {
                Success = true,
                Message = "Cart stored in Redis"
            };
        }

        public async Task<ResponseModel> GetRedisCart(string userId)
        {
            Console.WriteLine("Getting cart from Redis for user: " + userId);
            string key = $"cart:{userId}";

            var data = await _redisDb.StringGetAsync(key);

            if (data.IsNullOrEmpty)
            {
                var getFromDB = await GetCartFromDB(userId);
                if(getFromDB.Success) {
                    Console.WriteLine("Cart retrieved from DB for user: " + userId);
                    Console.WriteLine("Add Carts to redis for user: " + userId);
                    await StoreRedisCartAsync(userId, getFromDB.Books);
                    return new ResponseModel
                    {
                        Success = true,
                        Message = "Cart retrieved from DB",
                        Books = getFromDB.Books
                    };
                }
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cart is Empty"
                };
            }

            List<BookModel> json = JsonSerializer.Deserialize<List<BookModel>>(data);
            return new ResponseModel
            {
                Success = true,
                Message = "Cart present in Redis",
                Books = json
            };
        }

        public async Task<ResponseModel> RemoveFromRedisCart(string userId, BookModel request)
        {
            Console.WriteLine("Removing book from Redis cart for user: " + userId);
            string key = $"cart:{userId}";
            var data = await _redisDb.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return new ResponseModel
                {
                    Success = true,
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

        public async Task<ResponseModel> AddToCartAsync(string userId, List<BookModel> request)
        { 
            Console.WriteLine("Adding to DB cart for user: " + userId);
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<CartModel>.Update.Set(c => c.Carts, request);
            
            var result = await _repo.InsertCartAsync("Carts", filter, update, userId, request);
            return new ResponseModel
            {
                Success = true,
                Message = "Books added to DB cart"
            };
        }

        public async Task<ResponseModel> GetCartFromDB(string userId)
        {
            Console.WriteLine("Getting cart from DB for user: " + userId);
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, userId);
            var cartFromDB = await _repo.FindCartAsync("Carts", filter);

            if(cartFromDB.Count == 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "Cart is Empty"
                };
            }

            return new ResponseModel
            {
                Success = true,
                Message = "Cart retrieved from DB",
                Books = cartFromDB[0].Carts
            };
        }

        public async Task<ResponseModel> RemoveFromCartAsync(string userId, BookModel bookModel)
        {
            Console.WriteLine("Removing book from DB cart for user: " + userId);
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, userId);
            var cartFromDB = await _repo.FindCartAsync("Carts", filter);

            if(cartFromDB.Count == 0)
            {
                Console.WriteLine("Cart is empty for user: " + userId);
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cart is Empty"
                };
            }

            var carts = cartFromDB[0].Carts;

            carts = carts.Where(c => c.Id != bookModel.Id).ToList();

            var update = Builders<CartModel>.Update.Set(c => c.Carts, carts);

            await _repo.UpdateCartAsync("Carts", filter, update);
            return new ResponseModel
            {
                Success = true,
                Message = "Book removed from cart"
            };
        }

        public async Task<ResponseModel> Decrease(string userId, string bookId)
        {
            Console.WriteLine("Removing one book from Redis cart for user: " + userId+" of book: "+bookId);
            string key = $"cart:{userId}";
            var cartData = await _redisDb.StringGetAsync(key);
            if (cartData.IsNullOrEmpty)
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cart not found"
                };
            
            List<BookModel> cart = JsonSerializer.Deserialize<List<BookModel>>(cartData)!;
            var book = cart.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
                return new ResponseModel
                {
                    Success = false,
                    Message = "Book not found in cart"
                };

            book.Count = book.Count - 1;
            var updatedCartData = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync(key, updatedCartData);

            return new ResponseModel { Success = true, Message = "Book count updated successfully" };
        }
    }
}