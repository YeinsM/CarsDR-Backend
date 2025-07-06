using CarSpot.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

public class DomainEventHandlerFactory : IDomainEventHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IDomainEventHandler<T>> GetHandlers<T>() where T : IDomainEvent
    {
        return _serviceProvider.GetServices<IDomainEventHandler<T>>();
    }
}
