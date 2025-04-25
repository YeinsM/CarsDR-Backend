using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserPasswordChangedEvent : IDomainEvent
{
    public int UserId { get; }

    public UserPasswordChangedEvent(int userId)
    {
        UserId = userId;
    }
}
