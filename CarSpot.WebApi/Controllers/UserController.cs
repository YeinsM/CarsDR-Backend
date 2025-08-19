using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaginationService _paginationService;
        private readonly IEmailService _emailService;
        private readonly IEmailSettingsRepository _emailSettingsRepository;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IVehicleRepository _vehicleRepository;

        public UsersController(
            IUserRepository userRepository,
            IPaginationService paginationService,
            IEmailService emailService,
            IEmailSettingsRepository emailSettingsRepository,
            IConfiguration configuration,
            IJwtTokenGenerator jwtTokenGenerator,
            IVehicleRepository vehicleRepository)
        {
            _userRepository = userRepository;
            _paginationService = paginationService;
            _emailService = emailService;
            _emailSettingsRepository = emailSettingsRepository;
            _configuration = configuration;
            _jwtTokenGenerator = jwtTokenGenerator;
            _vehicleRepository = vehicleRepository;
        }


        [HttpGet]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<ActionResult<PaginatedResponse<UserDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _userRepository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(u => new UserDto(
                    u.Id,
                    u.Email,
                    u.Username,
                    u.Phone,
                    u.RoleId,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt,
                    u.BusinessId,
                    new List<VehicleDto>()
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



      
        [HttpGet("basic")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetAllBasic()
        {
            var users = await _userRepository.GetAllBasicAsync();

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
                new List<VehicleDto>()
            ));

            return Ok(response);
        }


        
        [HttpGet("{id:Guid}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var vehicles = await _vehicleRepository.GetAllAsync();
            var userVehicles = vehicles.Where(v => v.UserId == user.Id).ToList();

            var response = new UserDto(
                user.Id,
                user.Email,
                user.Username,
                user.Phone,
                user.RoleId,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt,
                user.BusinessId,
                userVehicles
            );

            return Ok(response);
        }


        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
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
         [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            User? user = await _userRepository.GetByEmailAsync(request.EmailOrUsername!);

            if (user == null)
                user = await _userRepository.GetByUsernameAsync(request.EmailOrUsername!);

            if (user == null || !user.Password.Verify(request.Password!))
                return Unauthorized(new { Status = 401, Message = "Invalid credentials" });

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new
            {
                Status = 200,
                Message = "Login successful",
                Token = token,
                Expires = DateTime.UtcNow.AddMinutes(60),
                UserId = user.Id
            });
        }


        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrUser")]
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
        [Authorize(Policy = "AdminOrUser")]
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
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
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


        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid token or user ID not found." });

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));

            if (user == null)
                return NotFound(new { message = "User not found." });

            var vehicles = await _vehicleRepository.GetAllAsync();
            var userVehicles = vehicles.Where(v => v.UserId == user.Id).ToList();

            var response = new UserDto(
                user.Id,
                user.Email,
                user.Username,
                user.Phone,
                user.RoleId,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt,
                user.BusinessId,
                userVehicles
            );

            return Ok(response);
        }
    }
}
