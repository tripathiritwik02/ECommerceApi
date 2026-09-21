namespace ECommerceApi.Services;

using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }    private UserResponse MaptoUserResponse(User user)
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
        var users = _context.Users.Include(u => u.Address).ToList();
        return users.Select(MaptoUserResponse).ToList();
    }

    public UserResponse? GetUserById(int id)
    {
        var user = _context.Users.Include(u => u.Address).FirstOrDefault(u => u.Id == id);
        if (user == null)
            return null;

        return MaptoUserResponse(user);
    }

    public void AddUser(UserRequest request)
    {
        var newUser = new User();
        UpdateUserFromRequest(newUser, request);
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }

    public bool UpdateUser(int id, UserRequest request)
    {
        var existingUser = _context.Users.Include(u => u.Address ).FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return false;

        UpdateUserFromRequest(existingUser, request);
        existingUser.Updatedat = DateTime.Now;
        _context.SaveChanges();
        return true;
    }
    public bool UserExist(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
}