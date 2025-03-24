using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _userRepository.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) != null)
        return BadRequest("Email already registered.");
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Email, passwordHash, request.FullName);
        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        return Unauthorized("Invalid credentials.");
        return Ok("Login successful.");
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.UpdateInfo(request.FullName);
        await _userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpPatch("{id:int}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();
        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
        return BadRequest("Incorrect old password.");
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(newPasswordHash);
        await _userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.Deactivate();
        await _userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpPatch("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.Activate();
        await _userRepository.UpdateAsync(user);
        return NoContent();

    }
}