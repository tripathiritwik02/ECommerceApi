namespace ECommerceApi.Dtos;

using ECommerceApi.Models;

public class UserResponse
{
    public int Id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
     public UserRole Role { get; set; } = UserRole.CUSTOMER;
    public AddressResponseDto? Address { get; set; }
}