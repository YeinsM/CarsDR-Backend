
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UserLoggedInDomainEvent : IDomainEvent
{
    public int UserId { get; }

    public UserLoggedInDomainEvent(int userId)
    {
        UserId = userId;
    }
}
