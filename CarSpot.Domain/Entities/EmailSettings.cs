// File: CarSpot.Domain/Entities/EmailSettings.cs
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class EmailSettings : BaseEntity
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; }
        public string FromPassword { get; set; }
        public string NickName { get; set; }

        public EmailSettings(string smtpServer, int smtpPort, string fromEmail, string fromPassword)
        {
            if (string.IsNullOrWhiteSpace(smtpServer))
                throw new ArgumentNullException(nameof(smtpServer), "SMTP server is required.");

            if (smtpPort <= 0)
                throw new ArgumentOutOfRangeException(nameof(smtpPort), "SMTP port must be greater than zero.");

            if (string.IsNullOrWhiteSpace(fromEmail))
                throw new ArgumentNullException(nameof(fromEmail), "From email is required.");

            if (string.IsNullOrWhiteSpace(fromPassword))
                throw new ArgumentNullException(nameof(fromPassword), "From password is required.");

            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            FromEmail = fromEmail;
            FromPassword = fromPassword;
        }

        public void Update(string smtpServer, int smtpPort, string fromEmail, string fromPassword)
        {
            if (string.IsNullOrWhiteSpace(smtpServer))
                throw new ArgumentNullException(nameof(smtpServer), "SMTP server is required.");

            if (smtpPort <= 0)
                throw new ArgumentOutOfRangeException(nameof(smtpPort), "SMTP port must be greater than zero.");

            if (string.IsNullOrWhiteSpace(fromEmail))
                throw new ArgumentNullException(nameof(fromEmail), "From email is required.");

            if (string.IsNullOrWhiteSpace(fromPassword))
                throw new ArgumentNullException(nameof(fromPassword), "From password is required.");

            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            FromEmail = fromEmail;
            FromPassword = fromPassword;

            SetUpdatedAt();
        }
    }
}
