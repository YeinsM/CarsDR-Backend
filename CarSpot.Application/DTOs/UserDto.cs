namespace CarSpot.Application.DTOs;

public record UserDto(
int Id,
string Email,
string Username,
bool IsActive,
DateTime CreatedAt,
DateTime? UpdatedAt
);
