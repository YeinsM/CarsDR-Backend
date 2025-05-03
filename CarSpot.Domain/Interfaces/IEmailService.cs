
using CarSpot.Domain.Entities;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, string nickName);
    string Body(User user);
}
