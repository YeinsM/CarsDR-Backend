namespace CarSpot.Application.DTOs;

public record UserDto(
Guid Id,
string Email,
string Username,
bool IsActive,
DateTime CreatedAt,
DateTime? UpdatedAt
);
