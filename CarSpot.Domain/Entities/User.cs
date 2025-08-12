using System.ComponentModel.DataAnnotations.Schema;
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;
using CarSpot.Domain.ValueObjects;

namespace CarSpot.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; set; }
        public string? Extension { get; set; }
        public string? CellPhone { get; set; }
        public string? Address { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public Guid BusinessId { get; set; }


        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public HashedPassword Password { get; private set; }

        [NotMapped]
        public string? ResetPassword { get; private set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";




        public User(string firstName, string lastName, string email, string? phone, string? extension, string? cellPhone, string? address, HashedPassword password, string username, Guid roleId)
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
            Phone = phone;
            Extension = extension;
            CellPhone = cellPhone;
            Address = address;
            Password = password;
            Username = username;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;


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
            AddDomainEvent(new UpdateUserEvent(Email, FullName));
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

        public void UpdateContactInfo(string? phone, string? extension, string? cellPhone, string? address)
        {
            Phone = phone;
            Extension = extension;
            CellPhone = cellPhone;
            Address = address;
        }

        public void NotifyUserRegistered()
        {
            AddDomainEvent(new UserRegisteredEvent(Email, FullName));
        }




    }
}

