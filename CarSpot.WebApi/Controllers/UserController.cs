using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IEmailSettingsRepository _emailSettingsRepository;
    private readonly IConfiguration _configuration;

    public UsersController(
        IUserRepository userRepository,
        IEmailService emailService,
        IConfiguration configuration,
        IEmailSettingsRepository emailSettingsRepository)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _configuration = configuration;
        _emailSettingsRepository = emailSettingsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        var response = users.Select(u => new UserDto(
            u.Id,
            u.Email,
            u.Username,
            u.Phone,
            u.RoleId,
            u.IsActive,
            u.CreatedAt,
            u.UpdatedAt,
            u.BusinessId,
            [.. u.Vehicles.Select(v => new VehicleDto(v.Id, v.VIN, v.Year, v.Color?.ToString(), v.ModelId))],
            [.. u.Comments.Select(c => new CommentResponse(c.Id, c.VehicleId, c.UserId, c.Content, c.CreatedAt))]
        ));

        return Ok(response);
    }


    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { Status = 404, Message = "User not found" });

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.Username,
            user.Phone,
            user.RoleId,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt,
            user.BusinessId,
            [.. user.Vehicles.Select(v => new VehicleDto(v.Id, v.VIN, v.Year, v.Color?.ToString(), v.ModelId))],
            [.. user.Comments.Select(c => new CommentResponse(c.Id, c.VehicleId, c.UserId, c.Content, c.CreatedAt))]
        );

        return Ok(userDto);
    }



    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest(new { Status = 400, Message = "Request body is required" });

            if (string.IsNullOrWhiteSpace(request.FirstName))
                return BadRequest(new { Status = 400, Message = "FirstName is required" });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { Status = 400, Message = "Email is required" });

            if (await _userRepository.IsEmailRegisteredAsync(request.Email))
                return BadRequest(new { Status = 400, Message = "Email already registered" });

            if (await _userRepository.GetByUsernameAsync(request.Username) != null)
                return BadRequest(new { Status = 400, Message = "Username already registered" });

            var hashedPassword = HashedPassword.Create(request.Password);

            var user = new User(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.Extension,
                request.CellPhone,
                request.Address,
                hashedPassword,
                request.Username,
                request.RoleId
            );

            await _userRepository.RegisterUserAsync(user);
            await _userRepository.SaveChangesAsync();

            /*var bodyMessage = _emailService.Body(user);
            var emailSettings = await _emailSettingsRepository.GetSettingsAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Welcome to CarSpot",
                bodyMessage,
                emailSettings?.NickName
            );*/

            return Ok(new { Status = 200, Message = "User registered successfully", UserId = user.Id });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                Status = 500,
                Error = "Database Error",
                Message = "Error saving user data",
                Details = ex.InnerException?.Message ?? ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = 500,
                Error = "Server Error",
                Message = "An unexpected error occurred",
                Details = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.EmailOrUsername);


        if (user == null)
            user = await _userRepository.GetByUsernameAsync(request.EmailOrUsername);

        if (user == null || !user.Password.Verify(request.Password))
            return Unauthorized(new { Status = 401, Message = "Invalid credentials" });

        return Ok(new { Status = 200, Message = "Login successful" });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { Status = 404, Message = "User not found" });

        user.UpdateBasicInfo(
            request.FirstName,
            request.LastName,
            request.Username
        );

        await _userRepository.UpdateAsync(id);

        return Ok(new { Status = 200, Message = "User updated successfully", User = new { user.Id, user.Username } });
    }

    [HttpPatch("{id:Guid}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { Status = 404, Message = "User not found" });

            user.ChangePassword(request.CurrentPassword, request.NewPassword, request.ConfirmNewPassword);

            await _userRepository.UpdateAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Status = 400, Message = ex.Message });
        }
    }

    [HttpPatch("{id:Guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { Status = 404, Message = "User not found" });

        user.Deactivate();
        await _userRepository.UpdateAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:Guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { Status = 404, Message = "User not found" });

        user.Activate();
        await _userRepository.UpdateAsync(id);
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
