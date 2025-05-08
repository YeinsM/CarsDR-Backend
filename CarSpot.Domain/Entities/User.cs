using CarSpot.Domain.Common;
using CarSpot.Domain.ValueObjects;
using CarSpot.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

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


        public string? Phone { get; set; }
        public string? CellPhone { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }        
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        
        public int RazónSocial{ get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public User(string firstName, string lastName, string email, HashedPassword password, string username, Guid roleId)
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
            if (roleId == Guid.Empty)
                throw new ArgumentNullException(nameof(roleId), "Role ID is required.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Username = username;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Register()
        {
            AddDomainEvent(new UserRegisteredEvent(Id, Email, FullName));
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
                throw new ArgumentNullException(nameof(newEmail), "Invalid email.");

            Email = newEmail;
        }

        public void ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (!Password.Verify(currentPassword))
                throw new ArgumentException("Current password is incorrect.");
            if (newPassword != confirmPassword)
                throw new ArgumentException("New password and confirmation do not match.");

            Password = HashedPassword.Create(newPassword);
            AddDomainEvent(new UserPasswordChangedEvent(Id));
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

            Password = HashedPassword.FromHashed(ResetPassword);
            ResetPassword = null;
        }

         public void UpdatePhone(string? phone)
        {
            Phone = phone;
        }

    }
}

