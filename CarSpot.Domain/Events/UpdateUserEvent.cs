using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events;

public class UpdateUserEvent : IDomainEvent
{

    public string Email { get; }
    public string FullName { get; }

    public UpdateUserEvent(string email, string fullName)
    {

        Email = email;
        FullName = fullName;
    }
}
