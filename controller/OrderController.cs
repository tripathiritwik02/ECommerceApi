using ECommerceApi.Dtos;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }
    [HttpPost("checkout/{userid}")]
    public IActionResult Checkout(int userid)
    {
        try
        {
            var order = _orderService.Checkout(userid);
            return Ok(ApiResponse<OrderResponse>.Success(order, "order placed"));
        }
        catch(InvalidOperationException e)
        {
            return BadRequest(ApiResponse<OrderResponse>.Failure(e.Message));
        }
    }
}