using CarSpot.Domain.Common;
using CarSpot.Domain.Common.Entities;
using BCrypt;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set;}
    public bool IsBlocked { get; private set; }
    public DateTime? BlockedAt { get; private set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        IsBlocked = false;
        BlockedAt = null;
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

    
        if (newPassword.Length < 8)
        throw new ArgumentException("The new password must be at least 8 characters long.");

        if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsLower) || !newPassword.Any(char.IsDigit))
        throw new ArgumentException("The new password must contain uppercase, lowercase and numbers..");

    
    string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

    
    PasswordHash = newPasswordHash;
    }

    public void Block()
    {
        if (IsBlocked)
            throw new InvalidOperationException("User is blocked.");

        IsBlocked = true;
        BlockedAt = DateTime.Now;
    }

    public void Unblock()
    {
        if (!IsBlocked)
            throw new InvalidOperationException("User is not blocked.");

        IsBlocked = false;
        BlockedAt = null;
    }

    public void CheckIfBlocked()
    {
        if (IsBlocked)
            throw new InvalidOperationException("The user is blocked and cannot perform this action..");
    }

}