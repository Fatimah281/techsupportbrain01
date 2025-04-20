
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechSupportBrain.Models;
using TechSupportBrain.Interfaces;

namespace TechSupportBrain.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JsonSerializerOptions _jsonOptions;

    public UsersController(IUserService userService)
    {
        _userService = userService;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _userService.Register(user))
            return Ok(true);
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] JsonElement loginData)
    {
        var email = loginData.GetProperty("email").GetString()!;
        var password = loginData.GetProperty("password").GetString()!;
        var user = await _userService.Login(email, password);
        if (user != null)
            return Ok(user);
        return Unauthorized();
    }
}