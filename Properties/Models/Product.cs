namespace ECommerceApi.Models;
using System.Numerics;

public class Product
{
    public int ProductId {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Category {get; set;} = string.Empty;
    public string ImageUrl {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public int StockQuantity{get; set;}
    public bool Active { get; set; } = true;

    public DateTime Createdat {get; set;}= DateTime.Now;
    public DateTime Updatedat {get; set;}= DateTime.Now; 
 
}