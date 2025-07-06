
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync<T>(string to, string subject, EmailTemplateType body, T entity, string? nickName);

}
