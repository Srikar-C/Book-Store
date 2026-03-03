using MongoDB.Driver;
using MongoDB.Bson;

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

        
    }
}