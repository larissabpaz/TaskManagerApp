using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AuthService _authService;

    public UserController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        var result = _authService.Register(user);
        return Ok(result);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var token = _authService.Login(user);
        return Ok(new { Token = token });
    }
}