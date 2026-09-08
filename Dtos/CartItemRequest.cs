using ECommerceApi.Models;

namespace ECommerceApi.Dtos;

public class CartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity {get; set;}
}

