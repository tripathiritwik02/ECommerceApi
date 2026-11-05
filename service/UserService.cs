namespace ECommerceApi.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Repostories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class UserService
{
    private readonly IRepostories<User> _repostories;
    private readonly IConfiguration _configuration;

    public UserService(IRepostories<User> repostories, IConfiguration configuration)
    {
        _repostories = repostories;
        _configuration = configuration;
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
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
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
        var users = _repostories.Query().Include(u => u.Address).ToList();
        return users.Select(MaptoUserResponse).ToList();
    }

    public UserResponse? GetUserById(int id)
    {
        var user = _repostories.Query().Include(u => u.Address).FirstOrDefault(u => u.Id == id);
        if (user == null)
            return null;

        return MaptoUserResponse(user);
    }

    public bool AddUser(UserRequest request)
    {
        if (_repostories.Query().Any(u => u.Name == request.Name))
        return false;


        var newUser = new User();
        UpdateUserFromRequest(newUser, request);
        _repostories.Add(newUser);
        _repostories.SaveChanges();
        return true;
    }

    public bool UpdateUser(int id, UserRequest request)
    {
        var existingUser = _repostories.Query().Include(u => u.Address).FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return false;

        UpdateUserFromRequest(existingUser, request);
        existingUser.Updatedat = DateTime.Now;
        _repostories.SaveChanges();
        return true;
    }

    public bool UserExist(int id)
{
    return _repostories.Query().Any(u => u.Id == id);
}

    public LoginResponse? Login(LoginRequest request)
    {
        var user = _repostories.Query().FirstOrDefault(u => u.Name == request.Name);
        if (user == null)
            return null;

        bool passwordmatches = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!passwordmatches)
            return null;

        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Name = user.Name,
            Role = user.Role.ToString()
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryminutes = Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"]);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryminutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}