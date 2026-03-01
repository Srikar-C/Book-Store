using System.Security.Claims;
using System.Text.Json;
using IdentityService.Services;
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

    [HttpPost("addCarts")]
    public async Task<IActionResult> AddBooksToCart([FromBody] CartModel request)
    {
        Console.WriteLine("userid-> ", request.UserId);

        Console.WriteLine("data of carts", JsonSerializer.Serialize(request));
        string str = await _bookService.AddToCart(request.Carts, request.UserId);
        if(str!="inserted")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Cart Successful"});

    }

    [HttpPost("getCarts")]
    public async Task<List<BooksModel>> GetUserCarts([FromBody] CartModel request)
    {
        Console.WriteLine("Entering to get user books", request.UserId);
       // string str = await _bookService.InsertBooks(request.UserId);
      //  Console.WriteLine(str);
        return await _bookService.GetCarts(request.UserId);
    }

    [HttpPost("removeCart")]
    public async Task<List<BooksModel>> RemoveOneFromCart([FromBody] BooksModel request)
    {
        Console.WriteLine("Entering in removing cart",request);
        return await _bookService.RemoveCart(request);
    }
}