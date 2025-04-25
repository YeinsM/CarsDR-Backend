using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserRegisteredEvent : IDomainEvent
{
    public int UserId { get; }
    public string Email { get; }
    public string FullName { get; }

    public UserRegisteredEvent(int userId, string email, string fullName)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
    }
}
