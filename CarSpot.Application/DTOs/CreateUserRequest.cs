namespace CarSpot.Application.DTOs;
public record CreateUserRequest(string Email, string Password, string FullName);
public record CreateUserRequest(
    [Required(ErrorMessage = "E-mail is required.")]
    [EmailAddress(ErrorMessage = "E-mail does not have a valid format.")]
    string Email,

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "The password must be at least 6 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$", 
        ErrorMessage = "The password must include uppercase, lowercase, numbers and a special character..")]
    string Password,

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "The full name cannot exceed 100 characters..")]
    string FullName
);