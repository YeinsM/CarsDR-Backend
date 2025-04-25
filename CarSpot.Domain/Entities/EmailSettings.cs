public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public string SmtpPort { get; set; } = "587";
    public string FromEmail { get; set; } = string.Empty;
    public string FromPassword{ get; set; } = string.Empty;
}
