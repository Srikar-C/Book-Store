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
        public async Task InsertAsync<UserModel>(string collectionName, UserModel data)
        {
            Console.WriteLine("Inside mongo creation");
            var collection = _database.GetCollection<UserModel>(collectionName);
            await collection.InsertOneAsync(data);
        }

        // 🔥 Generic Fetch
        public async Task<List<UserModel>> GetAsync<UserModel>(string collectionName, FilterDefinition<UserModel> filter)
        {
            Console.WriteLine("Inside mongo Find");
            var collection = _database.GetCollection<UserModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task ResetAsync(string collectionName, FilterDefinition<UserModel> filter, UpdateDefinition<UserModel> update)
        {
            Console.WriteLine("Inside mongo reset");
            var collection = _database.GetCollection<UserModel>(collectionName);
            await collection.UpdateOneAsync(filter,update);
        }
    }
}