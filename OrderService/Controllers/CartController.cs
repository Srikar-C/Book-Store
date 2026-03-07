using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;

[ApiController]
[Route("api/cart")]
public class CartController: ControllerBase
{
    private readonly CartService _service;
    public CartController(CartService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("addToCart")]
    public async Task<IActionResult> AddToCart([FromBody] List<BookModel> request)
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Received request to add to cart: " + request);
        var result = await _service.StoreCart(userId,request);

        if (result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [Authorize]
    [HttpPost("getCart")]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Getting request to get cart: " + userId);
        var result = await _service.GetCart(userId);
        if (result.Success)
        {
            return Ok(new {message= result.Message, data = result.Data});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [Authorize]
    [HttpDelete("removeFromCart/{bookId}")]
    public async Task<IActionResult> RemoveFromCart([FromRoute] string bookId)
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Received request to remove from cart: " + bookId);
        var result = await _service.RemoveFromCart(userId, new BookModel { Id = bookId });

        if (result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

}