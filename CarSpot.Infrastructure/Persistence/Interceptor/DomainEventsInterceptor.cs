
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;


using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarSpot.Infrastructure.Persistence.Interceptor;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IEmailService _emailService;

    public DomainEventsInterceptor(IEmailService emailService)
    {
        _emailService = emailService;
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

        foreach (var entityEntry in domainEntities)
        {
            var events = entityEntry.Entity.DomainEvents.ToList();
            entityEntry.Entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                switch (domainEvent)
                {
                    case UserRegisteredEvent registeredEvent:
                        await _emailService.SendEmailAsync(
                            registeredEvent.Email,
                            "Welcome to CarSpot!",
                            $"Hi {registeredEvent.FullName}, welcome to the app!"
                            , "CarSpot"
                        );
                        break;

                    case UserPasswordChangedEvent passwordChangedEvent:
                        
                        break;

                    case UserLoggedInDomainEvent loggedInEvent:
                        
                        break;
                }
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
