namespace CarSpot.Application.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string Username,
    string? Phone,
    Guid RoleId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid BusinessId,
    string? BusinessName
);
