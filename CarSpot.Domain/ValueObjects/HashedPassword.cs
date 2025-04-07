using System;
using System.Linq;
using BCrypt.Net;

namespace CarSpot.Domain.ValueObjects

{
    public class HashedPassword
    {
        public string Value { get; private set; }

        private HashedPassword(string hashedValue)
        {
            Value = hashedValue;
        }

        public static HashedPassword From(string rawPassword)
        {
            Validate(rawPassword);
            var hashed = BCrypt.Net.BCrypt.HashPassword(rawPassword);
            return new HashedPassword(hashed);
        }

        public bool Matches(string input) =>
            BCrypt.Net.BCrypt.Verify(input, Value);

        public HashedPassword Change(string currentRawPassword, string newPassword, string confirmPassword)
        {
            if (!Matches(currentRawPassword))
                throw new ArgumentException("Current password is incorrect.");

            if (newPassword != confirmPassword)
                throw new ArgumentException("Passwords do not match.");

            return From(newPassword);
        }

        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            if (password.Length < 6 ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit))
                throw new ArgumentException("Password must be at least 6 characters and contain upper, lower and digit.");
        }

        public override string ToString() => Value;
    
        
                
            
    }    

}
