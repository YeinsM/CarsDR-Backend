using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CarSpot.Domain.ValueObjects;
using CarSpot.Domain.Entities;



namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository _userRepository, IConfiguration _configuration, IRepository<User> _userRepositoryG) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _userRepositoryG.GetAllAsync());

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepositoryG.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (await _userRepository.IsEmailRegisteredAsync(request.Email))
            return BadRequest("Email already registered.");

        var hashedPassword = HashedPassword.FromHashed(request.Password);
        var user = new User(request.FirstName, request.LastName, request.Email, hashedPassword, request.Username);

        await _userRepository.RegisterUserAsync(user.FirstName, user.LastName, user.Email, user.Password, user.Username);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !user.Password.Verify(request.Password))
            return Unauthorized("Invalid credentials.");

        return Ok("Login successful.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        try
        {

            if (request == null)
                return BadRequest(new { Status = 400, Error = "Bad Request", Message = "Request body is required" });


            if (string.IsNullOrWhiteSpace(request.FirstName))
                return BadRequest(new { Status = 400, Error = "Validation Error", Message = "FirstName is required" });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { Status = 400, Error = "Validation Error", Message = "Email is required" });

            var hashedPassword = HashedPassword.FromHashed(request.Password); // Fix: Convert string password to HashedPassword

            var user = new User(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                password: hashedPassword, // Fix: Use hashedPassword instead of raw string
                username: request.Username
            );

            // Fix: Replace AddAsync with RegisterUserAsync
            await _userRepository.RegisterUserAsync(user.FirstName, user.LastName, user.Email, user.Password, user.Username);
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
                Message = "Error saving user data",
                Details = ex.InnerException?.Message
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {

        try
        {
            var user = await _userRepositoryG.GetByIdAsync(id);
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
                username: request.Username);

            return Ok(new
            {
                Status = 200,
                Message = "User updated successfully",
                User = new { user.Id, user.Username }
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Status = 404, Message = $"User with id {id} not found" });
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new { Status = 500, Message = "Database error" });
        }

    }

    [HttpPatch("{id:Guid}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            var user = await _userRepositoryG.GetByIdAsync(id);
            if (user == null) return NotFound();


            user.ChangePassword(
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword);

            await _userRepository.UpdateUserAsync(user.Id, user.FirstName, user.LastName, user.Username);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:Guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var user = await _userRepositoryG.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.Deactivate();
        await _userRepository.UpdateUserAsync(user.Id, user.FirstName, user.LastName, user.Username);
        return NoContent();
    }

    [HttpPatch("{id:Guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var user = await _userRepositoryG.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.Activate();
        await _userRepository.UpdateUserAsync(user.Id, user.FirstName, user.LastName, user.Username);
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

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail()
    {
        try
        {
            // Email credentials
            var smtpServer = "smtp.zoho.com";
            var smtpPort = 587;
            var fromEmail = "notifications@techbrains.com.do";
            var fromPassword = "Techbrains25@";

            // Email details
            var toEmail = "kellycasares13@gmail.com";
            var subject = "Test Email desde CarSpot";
            var body = "Esto es una prueba desde la aplicacion, tenemos email mija.";

            using var smtpClient = new System.Net.Mail.SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

            return Ok(new { Status = 200, Message = "Test email sent successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = 500, Error = "Email Error", Message = ex.Message });
        }
    }
}
