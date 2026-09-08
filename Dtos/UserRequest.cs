namespace ECommerceApi.Dtos;

public class UserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public AddressRequestDTO? Address { get; set; } 
}