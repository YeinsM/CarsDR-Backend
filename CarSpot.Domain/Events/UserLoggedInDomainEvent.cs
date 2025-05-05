
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserLoggedInDomainEvent : IDomainEvent
{
    public Guid UserId { get; }

    public UserLoggedInDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}
