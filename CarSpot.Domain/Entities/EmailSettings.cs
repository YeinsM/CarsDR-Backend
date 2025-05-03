using CarSpot.Domain.Common;

public class EmailSettings : BaseEntity
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string FromEmail { get; set; } = string.Empty;
    public string FromPassword{ get; set; } = string.Empty;
    public string? NickName { get; set; } 
}
