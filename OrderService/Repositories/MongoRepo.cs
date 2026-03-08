using MongoDB.Driver;

namespace OrderService.Repositories
{
    public class MongoRepo
    {
        private readonly IMongoDatabase _database;

        public MongoRepo(IMongoDatabase database)
        {
            this._database = database;
        }

        public async Task InsertAsync(string collectionName, OrderModel order)
        {
            var collection = _database.GetCollection<OrderModel>(collectionName);
            await collection.InsertOneAsync(order);
        }

        public async Task<List<OrderModel>> FindOrderAsync<OrderModel>(string collectionName, FilterDefinition<OrderModel> filter)
        {
            var collection = _database.GetCollection<OrderModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }


        public async Task<CartModel> InsertCartAsync(string collectionName, FilterDefinition<CartModel> filter, UpdateDefinition<CartModel> update, string userId, List<BookModel> request)
        {
            var collection = _database.GetCollection<CartModel>(collectionName);
            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            return new CartModel { UserId = userId, Carts = request };
        }

        public async Task<List<CartModel>> FindCartAsync(string collectionName, FilterDefinition<CartModel> filter)
        {
            var collection = _database.GetCollection<CartModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task UpdateCartAsync(string collectionName, FilterDefinition<CartModel> filter, UpdateDefinition<CartModel> update)
        {
            var collection = _database.GetCollection<CartModel>(collectionName);
            await collection.UpdateOneAsync(filter, update);
        }
    }
}