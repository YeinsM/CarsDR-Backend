

using CarSpot.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DomainEventsInterceptor(IDomainEventHandlerFactory handlerFactory) : SaveChangesInterceptor
{
    private readonly IDomainEventHandlerFactory _handlerFactory = handlerFactory;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return result;

        var domainEntities = eventData.Context.ChangeTracker
        .Entries<BaseEntity>()
        .Where(x => x.Entity.DomainEvents.Count != 0)
        .ToList();
        var domainEvents = domainEntities
        .SelectMany(x => x.Entity.DomainEvents)
        .ToList();
        // Limpiar los eventos después de obtenerlos
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
        foreach (IDomainEvent? domainEvent in domainEvents)
        {
            // Use reflection to dynamically call the generic method
            System.Reflection.MethodInfo? getHandlersMethod = _handlerFactory.GetType()
                .GetMethod("GetHandlers")
                ?.MakeGenericMethod(domainEvent.GetType());

            if (getHandlersMethod != null)
            {
                var handlers = (IEnumerable<object>)getHandlersMethod.Invoke(_handlerFactory, null)!;

                foreach (object handler in handlers)
                {
                    System.Reflection.MethodInfo? handleMethod = handler.GetType().GetMethod("HandleAsync");
                    if (handleMethod != null)
                    {
                        await (Task)handleMethod.Invoke(handler, [domainEvent])!;
                    }
                }
            }
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
