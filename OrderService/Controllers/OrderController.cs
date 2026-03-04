using System.Security.Claims;
using System.Text.Json;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;


[Route("api/orders")]
public class OrderController: ControllerBase
{
    private readonly OrderServices _orderService;
    private readonly CartServices _cartService;

    public OrderController(OrderServices orderService, CartServices cartService)
    {
        this._orderService = orderService;
        this._cartService = cartService;
    }


    [HttpPost("addCarts")]
    public async Task<IActionResult> AddBooksToCart([FromBody] CartModel request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("userid-> ", request.UserId);

        Console.WriteLine("data of carts", JsonSerializer.Serialize(request));
        string str = await _cartService.AddToCart(request, userId);
        if(str!="inserted")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Cart Successful"});

    }

    [HttpPost("getCarts")]
    public async Task<List<CartModel>> GetUserCarts([FromBody] CartRequestModel request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("Entering to get user books", request.UserId);
       // string str = await _bookService.InsertBooks(request.UserId);
      //  Console.WriteLine(str);
        return await _cartService.GetCarts(userId);
    }

    [HttpPost("removeCart")]
    public async Task<IActionResult> RemoveOneFromCart([FromBody] CartModel request)
    {
        Console.WriteLine("Entering in removing cart",request);
        await _cartService.RemoveCart(request);
        return Ok("removed");
    }

    [HttpPost("addOrders")]
    public async Task<IActionResult> PlaceOrders([FromBody] OrderModel request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("Entering into place orders",request);
        await _orderService.AddOrders(request.Orders, userId);
        return Ok(new {message= "added orders"});
    }

    [HttpPost("getOrders")]
    public async Task<List<CartModel>> GetUserOrders([FromBody] OrderModel request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("Entering to get user orders", request.UserId);
        return await _orderService.GetOrders(userId);
    }
}