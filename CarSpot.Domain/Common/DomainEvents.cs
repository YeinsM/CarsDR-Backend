// CarSpot.Domain/Common/DomainEvents.cs
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Common;

public static class DomainEvents
{
    private static readonly List<IDomainEvent> _events = new();

    public static IReadOnlyList<IDomainEvent> Events => _events;

    public static void Raise(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public static void Clear() => _events.Clear();
}
