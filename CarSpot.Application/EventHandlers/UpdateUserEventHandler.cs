using CarSpot.Application.Interfaces;
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;

public class UpdatedUserEventHandler : IDomainEventHandler<UpdateUserEvent>
{
    private readonly IEmailService _emailService;
    public UpdatedUserEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public async Task HandleAsync(UpdateUserEvent domainEvent)
    {
        await _emailService.SendEmailAsync(
        domainEvent.Email,
        "Your data has been updated!",
        EmailTemplateType.UpdateUser,
        new WelcomeEmailDto(domainEvent.FullName, domainEvent.Email),
        "Notifications"
        );
    }
}
