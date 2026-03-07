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

        public async Task<List<T>> FindOrderAsync<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }
    }
}