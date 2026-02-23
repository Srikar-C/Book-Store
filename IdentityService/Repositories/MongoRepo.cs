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
        public async Task InsertAsync<User>(string collectionName, User data)
        {
            var collection = _database.GetCollection<User>(collectionName);
            await collection.InsertOneAsync(data);
        }

        // 🔥 Generic Fetch
        public async Task<List<User>> GetAsync<User>(string collectionName, FilterDefinition<User> filter)
        {
            var collection = _database.GetCollection<User>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }
    }
}