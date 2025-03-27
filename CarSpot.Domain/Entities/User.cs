using CarSpot.Domain.Common;
using BCrypt;
using System.ComponentModel.DataAnnotations.Schema;

public class User : BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set;}
    [NotMapped]
    public string? ResetPassword { get; private set; }
    

    public User(string email, string password, string username)
    {
        Email = email;
        Password = password;
        Username = username;
        
    } 

    public void UpdateInfo(string username)
    {
        if(string.IsNullOrWhiteSpace(Username))
         throw new ArgumentNullException("Invalid fullname.");
           
        Username = username;
        SetUpdatedAt();
    }

    public void UpdateEmail(string newEmail)
    {
        if(string.IsNullOrWhiteSpace(newEmail))
         throw new ArgumentNullException("Invalid email.");
           
        Email = newEmail;
        SetUpdatedAt();
        
    }

    public void ChangePassword(string currentPassword, string? newPassword, string confirmNewPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("no characters found.");

        if (newPassword != confirmNewPassword)
            throw new ArgumentException("passwords do not match.");

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, Password))
            throw new ArgumentException("current password is incorrect.");

        if (newPassword.Length < 6)
            throw new ArgumentException("The new password must be at least 6 characters long.");

        if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsLower) || !newPassword.Any(char.IsDigit))
            throw new ArgumentException("The new password must contain uppercase, lowercase and numbers..");

        // Remove this line as it's creating a new variable with the same name
        // string newPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        SetUpdatedAt();
    }

     public void SetResetPassword(string newPassword, string confirmNewPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be null or empty.");

        if (newPassword != confirmNewPassword)
            throw new ArgumentException("Passwords do not match.");

        ValidatePassword(newPassword);

        ResetPassword = newPassword; 
    }

    public void ConfirmResetPassword()
    {
        if (string.IsNullOrWhiteSpace(ResetPassword))
            throw new InvalidOperationException("No password to reset.");

        Password = BCrypt.Net.BCrypt.HashPassword(ResetPassword);
        ResetPassword = null; 
        SetUpdatedAt();
    }

    private void ValidatePassword(string password)
    {
        if (password.Length < 6)
            throw new ArgumentException("The password must be at least 6 characters long.");

        if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            throw new ArgumentException("The password must contain uppercase, lowercase, and numbers.");
    }

}