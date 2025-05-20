using CarSpot.Domain.Common;

public interface IDomainEventHandlerFactory
{
    IEnumerable<IDomainEventHandler<T>> GetHandlers<T>() where T : IDomainEvent;
}