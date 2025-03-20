namespace CarSpot.Application.DTOs;
public record UpdateUserRequest(string FullName);
public record UpdateUserRequest(
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "The full name cannot exceed 100 characters..")]
    string FullName
);