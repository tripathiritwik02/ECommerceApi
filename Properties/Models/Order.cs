namespace ECommerceApi.Models;

public class Order
{
    public int Id {get; set;}
    public int UserId{get; set;}
    public DateTime OrderDate{get; set;} = DateTime.Now;
    public decimal TotalAmount{get; set;}
    public string Status{get; set;} = "Placed";

}