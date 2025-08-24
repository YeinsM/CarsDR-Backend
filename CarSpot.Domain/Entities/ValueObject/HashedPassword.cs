namespace CarSpot.Domain.ValueObjects
{
    public class HashedPassword
    {
        public string Value { get; private set; }

        private HashedPassword(string value)
        {
            Value = value;
        }

        public static HashedPassword Create(string password)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);
            return new HashedPassword(hashed);
        }

        public static HashedPassword FromHashed(string hashed)
        {
            return new HashedPassword(hashed);
        }

        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");
        }

        public bool Verify(string plaGuidext)
        {
            return BCrypt.Net.BCrypt.Verify(plaGuidext, Value);
        }
    }
}
