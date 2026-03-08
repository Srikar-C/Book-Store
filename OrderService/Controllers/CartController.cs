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
        var cartInsertInDB = await _service.AddToCartAsync(userId, request);
        var result = await _service.StoreRedisCartAsync(userId, request);

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
        var result = await _service.GetRedisCart(userId);
        if (result.Success)
        {
            return Ok(new {message= result.Message, data = result.Books});
        }
        else
        {
            var getCartFromDB = await _service.GetCartFromDB(userId);
            if(getCartFromDB.Success) {
                Console.WriteLine("Cart retrieved from DB for user: " + userId);
                Console.WriteLine("Add Carts to redis for user: " + userId);
                await _service.StoreRedisCartAsync(userId, getCartFromDB.Books);
                return Ok(new {message= getCartFromDB.Message, data = getCartFromDB.Books});
            }
            return BadRequest(new {message= result.Message});
        }
    }

    [Authorize]
    [HttpDelete("removeFromCart/{bookId}")]
    public async Task<IActionResult> RemoveFromCart([FromRoute] string bookId)
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Received request to remove from cart: " + bookId);
        var result = await _service.RemoveFromRedisCart(userId, new BookModel { Id = bookId });

        if (result.Success)
        {
            Console.WriteLine("Book removed from Redis cart for user: " + userId);
            Console.WriteLine("Removing book from DB cart for user: " + userId);
            var dbresult = await _service.RemoveFromCartAsync(userId, new BookModel { Id = bookId });
            Console.WriteLine("Book removed from DB cart for user: " + userId);
            if(dbresult.Success) {
                Console.WriteLine("Book removed from DB cart for user: " + userId);
                return Ok(new {message= dbresult.Message});
            } else {
                Console.WriteLine("Failed to remove book from DB cart for user: " + userId + ". Message: " + dbresult.Message);
                return BadRequest(new {message= result.Message});
            }
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

    [Authorize]
    [HttpDelete("decrementFromCart/{bookId}")]
    public async Task<IActionResult> DecrementFromCart([FromRoute] string bookId)
    {
        Console.WriteLine("Decrement the cart of book: "+bookId);
        var userId = User.FindFirst("id")?.Value;
        var result = await _service.Decrease(userId, bookId);
        if(result.Success)
        {
            return Ok(new {message= result.Message});
        }
        else
        {
            return BadRequest(new {message= result.Message});
        }
    }

}