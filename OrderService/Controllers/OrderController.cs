using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;

[ApiController]
[Route("api/order")]
public class OrderController: ControllerBase
{
    private readonly OrdService _service;
    public OrderController(OrdService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("placeOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] List<BookModel> request)
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Received request to place order for user: " + userId);
        var result = await _service.PlaceOrder(userId, request);

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
    [HttpGet("getOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var userId = User.FindFirst("id")?.Value;
        Console.WriteLine("Received request to get orders for user: " + userId);
        var orders = await _service.GetOrders(userId);
        if (orders.Success)
        {
            return Ok(new {message= orders.Message, data = orders.Data});
        }
        else
        {
            return BadRequest(new {message= orders.Message});
        }
    }
}