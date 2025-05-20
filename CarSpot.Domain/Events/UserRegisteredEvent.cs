using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserRegisteredEvent : IDomainEvent
{

    public string Email { get; }
    public string FullName { get; }

    public UserRegisteredEvent(string email, string fullName)
    {

        Email = email;
        FullName = fullName;
    }
}
