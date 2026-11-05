namespace ECommerceApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Services;
using ECommerceApi.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var result = _userService.Login(request);
        if (result == null)
            return Unauthorized("Invalid name or password");

        return Ok(result);
    }
}