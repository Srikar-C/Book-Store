
using System.ComponentModel;
using System.Security.Claims;
using System.Text.RegularExpressions;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace IdentityService.Services
{
    public class BookService
    {

        private readonly MongoRepo _repo;

        public BookService(MongoRepo repo)
        {
            this._repo = repo;
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

        public async Task<string> AddToCart(List<BooksModel> carts, string userId)
        {
            Console.WriteLine("userid in cart-> "+userId);
            Console.WriteLine("carts-> "+carts);

            await _repo.InsertMany("Carts", carts, userId);
            return "inserted";
        }
        public async Task<List<BooksModel>> GetCarts(string userid)
        {
            var filter = Builders<CartModel>.Filter.Eq(c=> c.UserId, userid);

            List<CartModel> cartDocs = await _repo.GetCartAsync("Carts",filter);
            List<BooksModel> allBooks = cartDocs
                .Where(c => c.Carts != null)
                .SelectMany(c => c.Carts)
                .ToList();
            return allBooks ?? new List<BooksModel>();
        }

        public async Task<List<BooksModel>> RemoveCart(BooksModel request)
        {
            var filter = Builders<CartModel>.Filter.Eq(c=>c.UserId, request.UserId);

            var update = Builders<CartModel>.Update.PullFilter(c=>c.Carts,b=>b.Id == request.Id);
            
            List<BooksModel> remBooks = await _repo.UpdateCart("Carts",filter,update);
            return remBooks;
        }
    }
}