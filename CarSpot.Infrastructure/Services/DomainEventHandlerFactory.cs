
using CarSpot.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

public class DomainEventHandlerFactory : IDomainEventHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;
    public DomainEventHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IEnumerable<object> GetHandlers(Type eventType)
    {
        // Crear el tipo genérico para el manejador
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        var handlers = _serviceProvider.GetServices(handlerType);
        return handlers.Cast<object>();
    }
}