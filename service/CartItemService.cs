using System.Text.Json;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services;

public class CartItemService
{
    private readonly List<CartItem> _cartItems;
    private readonly UserService _userService;
    private readonly ProductService _productService;

    public CartItemService(ProductService productService , UserService userService)
    {
        _productService = productService;
        _userService = userService;
       var filePath= Path.Combine(Directory.GetCurrentDirectory(), "Properties", "Models", "Data", "cartItem.json");
       var jsonData = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonData, options) ?? new List<CartItem>();
    }

    public bool AddToCart(int UserId, CartItemRequest request)
    {
        var product = _productService.GetProductById(request.ProductId);
        if(product==null)
        return false;

        if(product.StockQuantity < request.Quantity)
        return false;

        var existinguser = _userService.GetUserById(UserId);
        if(existinguser == null)
        return false;

        var existingCartItem = _cartItems.FirstOrDefault(c => c.UserId == UserId && c.ProductId == request.ProductId);
        if (existingCartItem != null)
        {
        existingCartItem.Quantity += request.Quantity;
        existingCartItem.Price = product.Price * existingCartItem.Quantity;
        }
        else
        {
            var newcartitem =new CartItem
               {
                Id = _cartItems.Count > 0 ? _cartItems.Max(c => c.Id) + 1 : 1,
                UserId = UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = product.Price * request.Quantity
            };
            _cartItems.Add(newcartitem);
        }
        return true;
    }

      public bool RemoveItemFromCart(int userId, int productId)
    {
        var cartItem = _cartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
        if (cartItem == null)
            return false;

        _cartItems.Remove(cartItem);
        return true;
    }

    public List<CartItem> GetCartItems(int userId)
    {
        return _cartItems.Where(c => c.UserId == userId).ToList();
    }
    
}