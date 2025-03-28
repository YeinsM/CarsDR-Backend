using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UsersController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
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
        //var password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.FirstName, request.LastName, request.Email, request.Password, request.Username);
        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized("Invalid credentials.");
        return Ok("Login successful.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest(new
                {
                    Status = 400,
                    Error = "Bad Request",
                    Message = "Request body is required"
                });

            
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new
                {
                    Status = 400,
                    Error = "Validation Error",
                    Message = "Email is required"
                });

            var user = new User(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                password: request.Password,
                username : request.Username
            );

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "User registered successfully",
                UserId = user.Id
            });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                Status = 500,
                Error = "Database Error",
                Message = "Error saving user data"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = 500,
                Error = "Server Error",
                Message = "An unexpected error occurred"
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new
                {
                    Status = 404,
                    Error = "Not Found",
                    Message = $"User with id {id} not found"
                });

            user.UpdateBasicInfo(
                firstName: request.FirstName,
                lastName: request.LastName,
                username: request.Username
            );

            await _userRepository.SaveChangesAsync();
            return Ok(new
            {
                Status = 200,
                Message = "User updated successfully",
                User = user
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = 500,
                Error = "Server Error",
                Message = "Error updating user data"
            });
        }
    }


    [HttpPatch("{id:int}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();


            user.ChangePassword(
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword);

            await _userRepository.UpdateAsync(user);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
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

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("Default"));
            await conn.OpenAsync();
            return Ok("Connected to Azure SQL successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Connection failed: {ex.Message}");
        }
    }


}