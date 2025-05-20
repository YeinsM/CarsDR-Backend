using CarSpot.Application.Interfaces;
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    public UserRegisteredEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public async Task HandleAsync(UserRegisteredEvent domainEvent)
    {
        await _emailService.SendEmailAsync(
        domainEvent.Email,
        "Welcome to CarSpot!",
        $"Hi {domainEvent.FullName}, welcome to the app!",
        "Notification"
        );
    }
}