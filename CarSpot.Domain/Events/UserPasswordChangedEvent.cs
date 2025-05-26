using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserPasswordChangedEvent : IDomainEvent
{
    public Guid UserId { get; }

    public UserPasswordChangedEvent(Guid userId)
    {
        UserId = userId;
    }
}
