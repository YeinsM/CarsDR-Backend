namespace CarSpot.Application.DTOs;
public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmNewPassword);



