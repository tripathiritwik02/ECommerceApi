namespace ECommerceApi.Models;

public class User
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public UserRole Role{get; set;} = UserRole.CUSTOMER;
    public Address? Address{get; set;}
    public DateTime Createdat {get; set;}= DateTime.Now;
    public DateTime Updatedat {get; set;}= DateTime.Now; 

}