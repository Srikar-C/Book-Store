using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;


[Route("api/orders")]
public class OrderController: ControllerBase
{
    private readonly OrderServices _orderService;

    public OrderController(OrderServices orderService)
    {
        this._orderService = orderService;
    }

    [HttpPost("addCarts")]
    public async Task<IActionResult> AddBooksToCart([FromBody] CartRequestModel request)
    {
        Console.WriteLine("userid-> ", request.UserId);

        Console.WriteLine("data of carts", JsonSerializer.Serialize(request));
        string str = await _orderService.AddToCart(request.Carts, request.UserId);
        if(str!="inserted")
        {
            return BadRequest(new {message = str});
        }
        return Ok(new {message = "Cart Successful"});

    }

    [HttpPost("getCarts")]
    public async Task<List<CartModel>> GetUserCarts([FromBody] CartRequestModel request)
    {
        Console.WriteLine("Entering to get user books", request.UserId);
       // string str = await _bookService.InsertBooks(request.UserId);
      //  Console.WriteLine(str);
        return await _orderService.GetCarts(request.UserId);
    }

    [HttpPost("removeCart")]
    public async Task<List<CartModel>> RemoveOneFromCart([FromBody] CartModel request)
    {
        Console.WriteLine("Entering in removing cart",request);
        return await _orderService.RemoveCart(request);
    }

    [HttpPost("addOrders")]
    public async Task<IActionResult> PlaceOrders([FromBody] OrderModel request)
    {
        Console.WriteLine("Entering into place orders",request);
        await _orderService.AddOrders(request.Orders, request.UserId);
        return Ok(new {message= "added orders"});
    }
}