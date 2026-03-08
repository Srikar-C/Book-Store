using MongoDB.Driver;

namespace CatalogueService.Repositories
{
    public class MongoRepo
    {
        private readonly IMongoDatabase _database;
        public MongoRepo(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoSettings:DatabaseName"]);
        }

        public async Task<List<BookModel>> FindBookAsync(string collectionName, FilterDefinition<BookModel> filter)
        {
            Console.WriteLine("Inside Finding Book");
            var collection = _database.GetCollection<BookModel>(collectionName);
            return await collection.Find(filter).ToListAsync();
        }

        internal async Task InsertOneAsync(string collectionName, BookModel request)
        {
            Console.WriteLine("Inside creating User");
            var collection = _database.GetCollection<BookModel>(collectionName);
            await collection.InsertOneAsync(request);
        }

        public async Task ReplaceBookAsync(string collectionName, FilterDefinition<BookModel> filter, BookModel request)
        {
            Console.WriteLine("Inside update Book");
            var collection = _database.GetCollection<BookModel>(collectionName);
            await collection.ReplaceOneAsync(filter, request);
        }

        public async Task UpdateQuantityAsync(string collectionName, FilterDefinition<BookModel> filter, UpdateDefinition<BookModel> update)
        {
            Console.WriteLine("Inside update quantity");
            var collection = _database.GetCollection<BookModel>(collectionName);
            await collection.UpdateOneAsync(filter, update);
        }
    }
}