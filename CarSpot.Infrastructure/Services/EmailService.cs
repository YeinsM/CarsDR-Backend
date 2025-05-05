using System.Net;
using System.Net.Mail;
using CarSpot.Domain.Entities;

public class EmailService : IEmailService
{
    private readonly IEmailSettingsRepository _emailSettingsRepository;

    public EmailService(IEmailSettingsRepository emailSettingsRepository)
    {
        _emailSettingsRepository = emailSettingsRepository;
    }

    public async Task SendEmailAsync(string to, string subject, string body, string nickName)
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

    public string Body(User user)
    {
        int Year = DateTime.Now.Year;
        string Email = $@"                
        <table width='100%' cellspacing='0' cellpadding='0'>                    
        <tr>                        
        <td align='center'>                            
        <table width='600' cellspacing='0' cellpadding='0'>                                
        <tr>                                    
        <td align='center' bgcolor='#0D3A52' style='padding: 40px 0;'>                                        
        <h1 style='color: #ffffff; font-size: 24px;'>Hola! {user.FullName}</h1>                                    
        </td>                                
        </tr>                                
        <tr>                                    
        <td><br>                                        
        Bienvenido a CarSpot, una nueva y mejor experiencia para encontrar o publicar tu vehiculo perfecto.                                                                            
         <td align='center' bgcolor='#0D3A52' style='padding: 20px 0;'>                                        
         <p style='color: #ffffff; font-size: 14px;'>© {Year} CarSpot - TechBrains Software .</p>                                    
         </td>                                
         </tr>                            
         </table>                        
         </td>                    
         </tr>                
         </table>";
        return Email;
    }
}


