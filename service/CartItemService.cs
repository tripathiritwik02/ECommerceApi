using System.Text.Json;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services;

public class CartItemService
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private readonly ProductService _productService;

    public CartItemService(AppDbContext context, UserService user, ProductService service)
    {
        _context = context;
        _userService = user;
        _productService  = service;
    }

    public bool AddToCart(int UserId, CartItemRequest request)
    {
        var product = _productService.GetProductById(request.ProductId);
        if(product==null)
        return false;

        if(product.StockQuantity < request.Quantity)
        return false;

        var existinguser = _userService.UserExist(UserId);
        if(existinguser == false)
        return false;

        var existingCartItem = _context.CartItems.FirstOrDefault(c => c.UserId == UserId && c.ProductId == request.ProductId);
        if (existingCartItem != null)
        {
        existingCartItem.Quantity += request.Quantity;
        existingCartItem.Price = product.Price * existingCartItem.Quantity;
        }
        else
        {
            var newcartitem =new CartItem
               {
                UserId = UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = product.Price * request.Quantity
            };
            _context.CartItems.Add(newcartitem);
        }
        _context.SaveChanges();
        return true;
    }

      public bool RemoveItemFromCart(int userId, int productId)
    {
        var cartItem = _context.CartItems
                                     .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
        if (cartItem == null)
            return false;

        _context.CartItems.Remove(cartItem);
        _context.SaveChanges();
        return true;
    }

    public List<CartItem> GetCartItems(int userId)
    {
        return _context.CartItems.Where(c => c.UserId == userId).ToList();
    }
    
}