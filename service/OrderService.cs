using ECommerceApi.Caching;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services;

public class OrderService
{
    private readonly AppDbContext _context;
    private readonly ProductCaching _cache;

    public OrderService(AppDbContext context,ProductCaching caching)
    {
        _context = context;
        _cache = caching;
    }

    public OrderResponse Checkout(int userId)
    {
        using var tx = _context.Database.BeginTransaction();

        try
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();
            if (cartItems.Count == 0)
                throw new InvalidOperationException("Cart is empty");

            var order = new Order
            {
                UserId = userId,
                TotalAmount = cartItems.Sum(c => c.Price)
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            var savedItems = new List<OrderItems>();

            foreach (var item in cartItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId)
                    ?? throw new InvalidOperationException($"Product {item.ProductId} not found");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Not enough stock for {product.Name}");

                var orderItem = new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);
                savedItems.Add(orderItem);

                product.StockQuantity -= item.Quantity;
                _context.SaveChanges();
            }   // foreach yahan band

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            tx.Commit();
            _cache.Clear();

            return new OrderResponse
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = savedItems.Select(i => new OrderitemResponse
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}