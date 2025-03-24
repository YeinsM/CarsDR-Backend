namespace CarSpot.Application.DTOs;

public record UserDto(
int Id,
string Email,
string FullName,
bool IsActive,
DateTime CreatedAt,
DateTime? UpdatedAt
);
