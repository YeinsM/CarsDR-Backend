using CarSpot.Domain.Common;

public interface IDomainEventHandlerFactory
{
    //IEnumerable<object> GetHandlers<T>() where T : IDomainEvent;
    IEnumerable<object> GetHandlers(Type eventType);
}