using CarSpot.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarSpot.Infrastructure.Persistence.Interceptor;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventHandlerFactory _handlerFactory;
    public DomainEventsInterceptor(IDomainEventHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return result;
        var domainEntities = eventData.Context.ChangeTracker
        .Entries<BaseEntity>()
        .Where(x => x.Entity.DomainEvents.Any())
        .ToList();
        var domainEvents = domainEntities
        .SelectMany(x => x.Entity.DomainEvents)
        .ToList();
        // Limpiar los eventos después de obtenerlos
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            // Use reflection to dynamically call the generic method
            var getHandlersMethod = _handlerFactory.GetType()
                .GetMethod("GetHandlers")
                ?.MakeGenericMethod(domainEvent.GetType());

            if (getHandlersMethod != null)
            {
                var handlers = (IEnumerable<object>)getHandlersMethod.Invoke(_handlerFactory, null)!;

                foreach (var handler in handlers)
                {
                    var handleMethod = handler.GetType().GetMethod("HandleAsync");
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
