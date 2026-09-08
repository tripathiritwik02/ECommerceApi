using ECommerceApi.Dtos;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartItemService _cartItemService;

    public CartController(CartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpPost("{userId}")]
    public IActionResult AddtoCart(int userId, [FromBody] CartItemRequest cartItemRequest)
    {
        bool added = _cartItemService.AddToCart(userId, cartItemRequest);
        if (added)
            return Ok("item added to cart");
        return BadRequest("could not add to cart (check product or user or stockquantity)");
    }

    [HttpDelete("{userId}/{productId}")]
    public IActionResult RemoveItemFromCart(int userId, int productId)
    {
        var deleted = _cartItemService.RemoveItemFromCart(userId, productId);
        if (deleted)
            return Ok("Product has been deleted from your cart");
        return NotFound("product cannot exist in your cart");
    }

    [HttpGet("{userId}")]
    public IActionResult GetItemFromCart(int userId)
    {
        var products = _cartItemService.GetCartItems(userId);
        return Ok(products);
    }
}