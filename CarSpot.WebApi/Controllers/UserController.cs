using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;

namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> CreatedUser([FromBody] CreatedUserRequest request)
    {
        var userId = await _userService.CreateUserAsync(request.Email, request.Password);
        CreatedAtAction(nameof(GetUser), new {id = userId}, userId);
    }

    [HttpPatch("{int:id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        await _userService.DeactivateUserAsync(id);
        return NoContent();

    }

}
public record CreatedUserRequest(string Email, string Password);