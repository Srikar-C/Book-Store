using MongoDB.Driver;
using MongoDB.Bson;

namespace IdentityService.Repositories
{
    public class MongoRepo
    {
        private readonly IMongoDatabase _database;

        public MongoRepo(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoSettings:DatabaseName"]);
        }

        public async Task<List<BooksModel>> GetAsync<BooksModel>(string collectionName, FilterDefinition<BooksModel> filter)
        {
            Console.WriteLine("Inside mongo Find");
            var collection = _database.GetCollection<BooksModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task InsertAsync(string collectionName, BooksModel data)
        {
            Console.WriteLine("Inside mongo cretaion");
            var collection = _database.GetCollection<BooksModel>(collectionName);
            await collection.InsertOneAsync(data);
        }

        public async Task InsertMany(string collectionName,List<BooksModel> carts, string userid)
        {
            Console.WriteLine("Inside mongo cretaion");
            var collection = _database.GetCollection<CartModel>(collectionName);
            CartModel cart = new CartModel()
            {
                UserId = userid,
                Carts = carts
            };
            await collection.InsertOneAsync(cart);
        }
        public async Task<List<CartModel>> GetCartAsync<CartModel>(string collectionName, FilterDefinition<CartModel> filter)
        {
            Console.WriteLine("Inside mongo Find");
            var collection = _database.GetCollection<CartModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<List<BooksModel>> UpdateCart(string collectionName, FilterDefinition<CartModel> filter, UpdateDefinition<CartModel> update)
        {
            Console.WriteLine("Inside remove one cart");
            var collection = _database.GetCollection<CartModel>(collectionName);
            await collection.UpdateOneAsync(filter,update);
            
            var updatedCart = await collection.Find(filter).FirstOrDefaultAsync();

            return updatedCart?.Carts ?? new List<BooksModel>();
        }
    }
}