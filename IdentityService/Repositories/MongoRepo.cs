using MongoDB.Driver;

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

        public async Task<List<RegisterModel>> FindUserAsync<RegisterModel>(string collectionName ,FilterDefinition<RegisterModel> filter)
        {
            Console.WriteLine("Inside Finding User");
            var collection = _database.GetCollection<RegisterModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        internal async Task InsertUserAsync(string collectionName, RegisterModel request)
        {
            Console.WriteLine("Inside creating User");
            var collection = _database.GetCollection<RegisterModel>(collectionName);
            await collection.InsertOneAsync(request);
        }
    }
}