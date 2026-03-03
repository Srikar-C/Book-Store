using System.Security.Claims;
using System.Text.Json;
using CatalogueService.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/book")]
public class BooksController: ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        this._bookService = bookService;
    }

    [HttpPost("getbooks")]
    public async Task<List<BooksModel>> GetAllBooks([FromBody] BooksModel request)
    {
        Console.WriteLine("Entering to getall books", request.UserId);
       // string str = await _bookService.InsertBooks(request.UserId);
      //  Console.WriteLine(str);
        return await _bookService.GetBooks();
    }

    
}