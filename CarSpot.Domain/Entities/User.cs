using CarSpot.Domain.Common;
using CarSpot.Domain.Common.Entities;
using BCrypt;

public class User : BaseEntity
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set;}
    [NotMapped]
    public string ResetPassword { get; private set; }
    

    public User(string email, string passwordHash, string fullName)
    {
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        
    } 

    public void UpdateInfo(string fullName)
    {
        if(string.IsNullOrWhiteSpace(FullName))
         throw new ArgumentNullException("Invalid fullname.");
           
        FullName = fullName;
        SetUpdatedAt();
    }

    public void UpdateEmail(string newEmail)
    {
        if(string.IsNullOrWhiteSpace(newEmail))
         throw new ArgumentNullException("Invalid email.");
           
        Email = newEmail;
        SetUpdatedAt();
        
    }

    public void ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
        throw new ArgumentException("no characters found.");

    
        if (newPassword != confirmNewPassword)
        throw new ArgumentException("passwords do not match.");

    
        if (!BCrypt.Net.BCrypt.Verify(currentPassword, PasswordHash))
        throw new ArgumentException("current password is incorrect.");

    
        if (newPassword.Length < 6)
        throw new ArgumentException("The new password must be at least 6 characters long.");

        if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsLower) || !newPassword.Any(char.IsDigit))
        throw new ArgumentException("The new password must contain uppercase, lowercase and numbers..");

    
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

    
        PasswordHash = newPasswordHash;
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

        PasswordHash = BCrypt.Net.BCrypt.HashPassword(ResetPassword);
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