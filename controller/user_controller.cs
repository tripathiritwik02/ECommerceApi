namespace ECommerceApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Services;
using ECommerceApi.Models;
using ECommerceApi.Dtos;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
            return NotFound($"User with id {id} not found");
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserRequest request)
    {
        _userService.AddUser(request);
        return Ok("user added");
    }

[HttpPut("{id}")]

    public IActionResult Updateuser(int id, [FromBody] UserRequest request)
    {
        bool updated =_userService.UpdateUser(id,request);
        if (updated)
        return Ok("user updated");
        return NotFound($"user with id {id} not found");
    }
}