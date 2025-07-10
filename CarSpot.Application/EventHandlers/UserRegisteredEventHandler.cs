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
        Console.WriteLine($"UserRegisteredEventHandler: Processing event for user {domainEvent.Email}");
        
        await _emailService.SendEmailAsync(
        domainEvent.Email,
        "Welcome to CarSpot!",
        EmailTemplateType.Welcome,
        new WelcomeEmailDto(domainEvent.FullName, domainEvent.Email),
        "Notifications"
        );
    }
}
