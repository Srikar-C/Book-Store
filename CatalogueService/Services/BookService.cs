using CatalogueService.Repositories;
using MongoDB.Driver;

namespace CatalogueService.Services
{
    public class BookService
    {
        private readonly MongoRepo _repo;

        public BookService(MongoRepo repo)
        {
            this._repo = repo;
        }

        public async Task<ResponseModel> AddBook(BookModel request)
        {
            var titleFilter = Builders<BookModel>.Filter.Eq(u => u.Title, request.Title);
            var existingBook = await _repo.FindBookAsync("Books", titleFilter);

            if(existingBook.Count > 0)
            {
                Console.WriteLine("Book with title already exists: " + request.Title);
                return new ResponseModel { Success = false, Message = "Book with this title already exists" };
            }
            
            await _repo.InsertOneAsync("Books",request);
            Console.WriteLine("Book added successfully: " + request.Title);
            return new ResponseModel { Success = true, Message = "Book added successfully" };
        }

        public async Task<ResponseModel> GetBooks()
        {
            var filter = Builders<BookModel>.Filter.Empty;
            var books = await _repo.FindBookAsync("Books", filter);
            Console.WriteLine("Books retrieved successfully. Count: " + books.Count);
            return new ResponseModel { Success = true, Message = "Books retrieved successfully", Data = books };
        }
    }
}