using MongoDB.Driver;
using MongoDB.Bson;

namespace OrderService.Repositories
{
    public class MongoRepo
    {
        private readonly IMongoDatabase _database;

        public MongoRepo(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoSettings:DatabaseName"]);
        }

        public async Task InsertMany(string collectionName,List<CartModel> carts, string userid)
        {
            Console.WriteLine("Inside mongo cretaion");
            var collection = _database.GetCollection<CartRequestModel>(collectionName);
            CartRequestModel cart = new CartRequestModel()
            {
                UserId = userid,
                Carts = carts
            };
            await collection.InsertOneAsync(cart);
        }
        public async Task<List<CartRequestModel>> GetCartAsync<CartRequestModel>(string collectionName, FilterDefinition<CartRequestModel> filter)
        {
            Console.WriteLine("Inside mongo Find");
            var collection = _database.GetCollection<CartRequestModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<List<CartModel>> UpdateCart(string collectionName, FilterDefinition<CartRequestModel> filter, UpdateDefinition<CartRequestModel> update)
        {
            Console.WriteLine("Inside remove one cart");
            var collection = _database.GetCollection<CartRequestModel>(collectionName);
            await collection.UpdateOneAsync(filter,update);
            
            var updatedCart = await collection.Find(filter).FirstOrDefaultAsync();

            return updatedCart?.Carts ?? new List<CartModel>();
        }

        public async Task InsertOrdersMany(string collectionName,List<CartModel> orders, string userid)
        {
            Console.WriteLine("Inside mongo cretaion");
            var collection = _database.GetCollection<OrderModel>(collectionName);
            OrderModel order = new OrderModel()
            {
                UserId = userid,
                Orders = orders
            };
            await collection.InsertOneAsync(order);
        }

        public async Task<List<OrderModel>> GetOrderAsync<OrderModel>(string collectionName, FilterDefinition<OrderModel> filter)
        {
            Console.WriteLine("Inside mongo Find");
            var collection = _database.GetCollection<OrderModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }
    }
}