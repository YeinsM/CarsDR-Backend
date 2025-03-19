namespace CarSpot.Application.DTOs;

public record UserDto(
    int Id,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt

);

