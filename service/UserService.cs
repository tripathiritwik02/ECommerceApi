namespace ECommerceApi.Services;

using ECommerceApi.Dtos;
using ECommerceApi.Models;
using System.Text.Json;

public class UserService
{
    private readonly List<User> _users;

    public UserService()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "Models", "Data", "users.json");
        var jsonData = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        _users = JsonSerializer.Deserialize<List<User>>(jsonData, options) ?? new List<User>();
    }

    private UserResponse MaptoUserResponse(User user)
    {
        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.Name,
            Role = user.Role
        };

        if (user.Address != null)
        {
            response.Address = new AddressResponseDto
            {
                Street = user.Address.Street,
                City = user.Address.City,
                State = user.Address.State,
                Country = user.Address.Country,
                Zipcode = user.Address.Zipcode
            };
        }

        return response;
    }

    private void UpdateUserFromRequest(User user, UserRequest request)
    {
        user.Name = request.Name;
        user.Password = request.Password;

        if (request.Address != null)
        {
            if (user.Address == null)
            {
                user.Address = new Address();
            }

            user.Address.Country = request.Address.Country;
        }
    }

    public List<UserResponse> GetAllUsers()
    {
        return _users.Select(MaptoUserResponse).ToList();
    }

    public UserResponse? GetUserById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return null;

        return MaptoUserResponse(user);
    }

    public void AddUser(UserRequest request)
    {
        var newUser = new User();
        UpdateUserFromRequest(newUser, request);
        newUser.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(newUser);
    }

    public bool UpdateUser(int id, UserRequest request)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return false;

        UpdateUserFromRequest(existingUser, request);
        existingUser.Updatedat = DateTime.Now;

        return true;
    }
}