
using System.ComponentModel;
using System.Security.Claims;
using System.Text.RegularExpressions;
using CatalogueService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace CatalogueService.Services
{
    public class BookService
    {

        private readonly IDistributedCache _cache;
        private readonly MongoRepo _repo;

        public BookService(MongoRepo repo, IDistributedCache cache)
        {
            this._repo = repo;
            this._cache = cache;
        }
        
        public async Task<List<BooksModel>> GetBooks()
        {
            var filter = Builders<BooksModel>.Filter.Empty;

            return await _repo.GetAsync("Books",filter);
        }

        public async Task<string> InsertBooks(string userid)
        {
            Console.WriteLine("userid in books-> :", userid);
            BooksModel newbook = new BooksModel()
            {
                UserId = userid,
                BookName = "book1",
                BookAuthor = "autho1",
                BookDesc = "an book with author",
                BookPrice = 500,
                BookSelect = false,
            };

            await _repo.InsertAsync("Books",newbook);
            return "inserted";
        }

       
    }
}