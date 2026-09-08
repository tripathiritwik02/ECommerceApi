namespace ECommerceApi.Dtos;

public class AddressResponseDto{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Zipcode { get; set; } = string.Empty;
}