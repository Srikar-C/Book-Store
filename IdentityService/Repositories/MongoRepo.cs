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

        // 🔥 Generic Insert
        public async Task InsertAsync<T>(string collectionName, T data)
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(data);
        }

        // 🔥 Generic Fetch
        public async Task<List<T>> GetAsync<T>(
            string collectionName,
            FilterDefinition<T> filter)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }
    }
}