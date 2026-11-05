namespace ECommerceApi.Dtos;

public class OrderResponse
{
    public int Id {get; set;}
    public decimal TotalAmount{ get; set;}
    public string Status{get; set;} = String.Empty;
    public List<OrderitemResponse> Items {get; set;} = new List<OrderitemResponse>();
}

public class OrderitemResponse
{
    public int ProductId{get; set;}
    public int Quantity{get; set;}
    public decimal Price {get; set;}
}