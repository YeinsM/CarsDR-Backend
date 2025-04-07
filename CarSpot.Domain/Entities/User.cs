using CarSpot.Domain.Common;
using BCrypt;
using System.ComponentModel.DataAnnotations.Schema;
using CarSpot.Domain.ValueObjects;




namespace CarSpot.Domain.Entities
{



    public class User : BaseEntity
    {

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public HashedPassword Password { get; private set; }


        [NotMapped]
        public string? ResetPassword { get; private set; }


        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";


        public User(string firstName, string lastName, string email, HashedPassword password, string username)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName), "First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName), "Last name is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Email is required.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username is required.");

            if (password is null)
                throw new ArgumentNullException(nameof(password), "Password is required.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Username = username;

        
        }




        public void UpdateBasicInfo(string firstName, string lastName, string username)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName), "Invalid first name.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName), "Invalid last name.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Invalid username.");

            FirstName = firstName;
            LastName = lastName;

            Username = username;
            
        }


        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentNullException("Invalid email.");

            Email = newEmail;
            

        }

        public void ChangePassword(string current, string newPass, string confirm)
        {
            Password = Password.Change(current, newPass, confirm);
            
        }


        public void SetResetPassword(string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
                throw new ArgumentException("Passwords do not match.");

            HashedPassword.Validate(newPassword);
            ResetPassword = newPassword;
        }

        public void ConfirmResetPassword()
        {
            if (string.IsNullOrWhiteSpace(ResetPassword))
                throw new InvalidOperationException("No password to reset.");

            Password = HashedPassword.From(ResetPassword);
            ResetPassword = null;
            
        }

    }
}