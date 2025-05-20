
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, string? nickName);
    string Body(User user);
}
