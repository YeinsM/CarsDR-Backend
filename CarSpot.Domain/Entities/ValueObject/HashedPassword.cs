using BCrypt.Net;

namespace CarSpot.Domain.ValueObjects;

public class HashedPassword
{
    private readonly string _value;

    private HashedPassword(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static HashedPassword Create(string plainPassword)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        return new HashedPassword(hashed);
    }

    public bool Verify(string plainPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, _value);
    }

    public override string ToString() => _value;

    public static implicit operator string(HashedPassword hashedPassword)
        => hashedPassword._value;
}
