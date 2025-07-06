using System.Net;
using System.Net.Mail;
using CarSpot.Application.Interfaces;

namespace CarSpot.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailSettingsRepository _emailSettingsRepository;
    public EmailService(IEmailSettingsRepository emailSettingsRepository)
    {
        _emailSettingsRepository = emailSettingsRepository;
    }
    public async Task SendEmailAsync<T>(string to, string subject, EmailTemplateType templateType, T entity, string? nickName = null)
    {
        var emailSettings = await _emailSettingsRepository.GetSettingsByNickNameAsync(nickName);
        if (emailSettings == null)
            throw new InvalidOperationException($"Email settings with nickName '{nickName}' not found.");
        var smtpClient = new SmtpClient(emailSettings.SmtpServer)
        {
            Port = emailSettings.SmtpPort,
            Credentials = new NetworkCredential(emailSettings.FromEmail, emailSettings.FromPassword),
            EnableSsl = true,
        };
        string body = GenerateEmailBody(templateType, entity);
        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSettings.FromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);
        await smtpClient.SendMailAsync(mailMessage);
    }
    private string GenerateEmailBody<T>(EmailTemplateType templateType, T entity)
    {
        var bodyBuilder = EmailBodyBuilderFactory.GetBuilder<T>(templateType);
        string content = bodyBuilder.Build(entity);
        string year = DateTime.Now.Year.ToString();
        return $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                <meta charset=""UTF-8"">
                <title>CarSpot - Notificación</title>
                <style>
                body {{
                margin: 0;
                padding: 0;
                font-family: Poppins, sans-serif;
                background-color: #f4f4f4;
                }}
                .email-container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                background-color: #0D3A52;
                padding: 20px;
                text-align: center;
                color: #ffffff;
                font-size: 24px;
                }}
                .content {{
                padding: 20px;
                text-align: left;
                color: #333333;
                font-size: 16px;
                line-height: 1.5;
                }}
                .footer {{
                background-color: #0D3A52;
                color: #ffffff;
                text-align: center;
                padding: 15px;
                font-size: 14px;
                }}
                </style>
                </head>
                <body>
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" class=""email-container"">
                <tr>
                <td class=""header"">
                Notificación de CarSpot
                </td>
                </tr>
                <tr>
                <td class=""content"">
                {content}
                </td>
                </tr>
                <tr>
                <td class=""footer"">
                &copy; {year} CarSpot - TechBrains Software
                </td>
                </tr>
                </table>
                </body>
                </html>
                ";
    }
}
