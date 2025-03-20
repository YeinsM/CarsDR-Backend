namespace CarSpot.Application.DTOs;
public record LoginRequest(string Email, string Password);
public record LoginRequest(
    [Required(ErrorMessage = "E-mail is required.")]
    [EmailAddress(ErrorMessage = "E-mail does not have a valid format.")]
    string Email,

    [Required(ErrorMessage = "Password is required.")]
    string Password
);