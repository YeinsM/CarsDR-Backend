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

        // Constructor para inicializar el usuario.
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

        // Método de registro que genera un evento de dominio.
        public void Register()
        {
            // Suponiendo que DomainEvents es una lista de eventos de dominio, se agrega un nuevo evento.
            AddDomainEvent(new UserRegisteredEvent(Id, Email));
        }

        // Actualiza la información básica del usuario.
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

        // Actualiza el email del usuario.
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentNullException(nameof(newEmail), "Invalid email.");

            Email = newEmail;
        }

        // Cambia la contraseña del usuario.
        public void ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (!Password.Verify(currentPassword))
                throw new ArgumentException("Current password is incorrect.");
            if (newPassword != confirmPassword)
                throw new ArgumentException("New password and confirmation do not match.");

            Password = HashedPassword.Create(newPassword);
            AddDomainEvent(new UserPasswordChangedEvent(Id));
        }

        // Establece la nueva contraseña para el proceso de restablecimiento.
        public void SetResetPassword(string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
                throw new ArgumentException("Passwords do not match.");

            HashedPassword.Validate(newPassword);
            ResetPassword = newPassword;
        }

        // Confirma el restablecimiento de la contraseña.
        public void ConfirmResetPassword()
        {
            if (string.IsNullOrWhiteSpace(ResetPassword))
                throw new InvalidOperationException("No password to reset.");

            Password = HashedPassword.From(ResetPassword);
            ResetPassword = null;
        }

        // Método para agregar eventos de dominio a una lista de eventos.
        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            // Aquí debes tener una colección o lista para almacenar los eventos de dominio.
            DomainEvents.Add(domainEvent);
        }
    }
}
