using CatalogueService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/books")]
public class BookController: ControllerBase
{
    private readonly BookService _service;

    public BookController(BookService service)
    {
        _service = service;
    }

    [HttpPost("addBooks")]
    public async Task<IActionResult> AddBooks([FromBody] BookModel request)
    {
        Console.WriteLine("Received request to add book: " + request.Title);
        var result = await _service.AddBook(request);

        if (result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [HttpPost("getBooks")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _service.GetBooks();
        if(books.Success)
        {
            Console.WriteLine("Books retrieved successfully. Count: " + books.Data?.Count);
            return Ok(new { message = books.Message, data = books.Data });
        }
        else
        {
            Console.WriteLine("Failed to retrieve books: " + books.Message);
            return BadRequest(new { message = books.Message });
        }
    }
}