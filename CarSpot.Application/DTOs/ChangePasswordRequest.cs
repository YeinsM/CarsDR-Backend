namespace CarSpot.Application.DTOs;
public record ChangePasswordRequest(string OldPassword, string NewPassword);



public record ChangePasswordRequest(
    [Required(ErrorMessage = "The current password is required.")]
    string OldPassword,

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(6, ErrorMessage = "The new password must be at least 6 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$", 
        ErrorMessage = "The password must include uppercase, lowercase, numbers and a special character..")]
    string NewPassword
);

